namespace Compiler.Assembly
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;

    public class Procedure : AssemblyObject
    {
        readonly Dictionary<string, int> parameterOffsets = new Dictionary<string, int>();

        private static int currentOffset;

        private readonly List<string> parameters = new List<string>();

        private SymbolTable symbolTable;

        public Procedure(string name, IEnumerable<string> parameters, AssemblyFile assemblyFile, SymbolTable symbolTable)
        {
            this.Name = name;
            this.AssemblyFile = assemblyFile;
            this.symbolTable = symbolTable;
            this.Blocks = new List<Block>();
            this.parameters.AddRange(parameters);
        }

        public string Name { get; set; }

        public IList<Block> Blocks { get; private set; }

        public AssemblyFile AssemblyFile { get; private set; }

        public int NumberOfLocalVariables
        {
            get
            {
                return (Math.Abs(currentOffset) + 8) / 8;
            }
        }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine(this.Name + " PROC");

            writer.WriteLine("push rbp");

            IEnumerable<Register> usedRegisters = new Register[0];

            if (this.Name != "main")
            {
                usedRegisters = GetUsedRegisters();
            }

            foreach (var usedRegister in usedRegisters)
            {
                if (RegisterUtility.IsXMM(usedRegister))
                {
                    writer.WriteLine("MOVD R10, {0}", usedRegister);
                    writer.WriteLine("PUSH R10");
                }
                else
                {
                    writer.WriteLine("PUSH {0}", usedRegister);
                }
            }

            writer.WriteLine("mov rbp, rsp");
            writer.WriteLine("sub rsp, {0}", 16 * Math.Ceiling(((double)this.NumberOfLocalVariables * 8) / 16)); // Allocate stackframe space, and allign to 16 bit.

            int parameterOffset = 48 + 8 * usedRegisters.Count() + Math.Max(0, 8 * (this.parameters.Count - CallingConvention.NumberOfRegisterParams));
            foreach (var parameter in parameters.Skip(CallingConvention.NumberOfRegisterParams))
            {
                var variableSymbol = (VariableSymbol)this.symbolTable.GetSymbol(parameter, SymbolType.Variable);

                if (variableSymbol.Register.HasValue)
                {
                    if (RegisterUtility.IsXMM(variableSymbol.Register.Value))
                    {
                        writer.WriteLine("MOV R10, [RBP + {0}]", parameterOffset);
                        writer.WriteLine("MOVD {0}, R10", variableSymbol.Register.Value);
                    }
                    else
                    {
                        writer.WriteLine("MOV {0}, [RBP + {1}]", variableSymbol.Register.Value, parameterOffset);    
                    }
                }
                else
                {
                    writer.WriteLine("MOV R10, [RBP + {0}]", parameterOffset);
                    writer.WriteLine("MOV {0}, R10", this.GetVariableLocation(parameter));
                }

                parameterOffset -= 8;
            }

            foreach (var block in this.Blocks)
            {
                block.Write(writer);
            }

            writer.WriteLine("{0}exit:", this.Name);

            if (this.Name == "main")
            {
                writer.WriteLine("call exit");
            }

            writer.WriteLine("mov rsp, rbp");
            foreach (var usedRegister in usedRegisters.Reverse())
            {
                if (RegisterUtility.IsXMM(usedRegister))
                {
                    writer.WriteLine("POP R10");
                    writer.WriteLine("MOVD {0}, R10", usedRegister);
                }
                else
                {
                    writer.WriteLine("POP {0}", usedRegister);
                }
            }
            

            writer.WriteLine("pop rbp");
            writer.WriteLine("ret");
            writer.WriteLine(this.Name + " ENDP");
        }

        public MemoryOperand GetVariableLocation(VariableSymbol variableSymbol)
        {
            return this.GetVariableLocation(variableSymbol.Name);
        }

        private MemoryOperand GetVariableLocation(string variableName)
        {
            int varOffset;

            if (this.parameterOffsets.ContainsKey(variableName))
            {
                varOffset = this.parameterOffsets[variableName];
            }
            else
            {
                varOffset = currentOffset -= 8;
                this.parameterOffsets[variableName] = currentOffset;
            }

            return new MemoryOperand(Register.RBP, varOffset);
        }

        private IEnumerable<Register> GetUsedRegisters()
        {
            var usedRegisters = new HashSet<Register>();

            foreach (var block in Blocks)
            {
                foreach (var instruction in block.Instructions)
                {
                    var binaryOpCodeInstruction = instruction as BinaryOpCodeInstruction;
                    if (binaryOpCodeInstruction != null)
                    {
                        var operand1 = binaryOpCodeInstruction.Argument1 as RegisterOperand;
                        if (operand1 != null) usedRegisters.Add(operand1.Register);

                        var operand2 = binaryOpCodeInstruction.Argument2 as RegisterOperand;
                        if (operand2 != null) usedRegisters.Add(operand2.Register);
                    }

                    var singleOpcodeInstruction = instruction as SingleOpcodeInstruction;
                    if (singleOpcodeInstruction != null)
                    {
                        var operand1 = singleOpcodeInstruction.Argument as RegisterOperand;
                        if (operand1 != null) usedRegisters.Add(operand1.Register);
                    }
                }
            }

            // Don't reset return value register.
            usedRegisters.Remove(Register.RAX);

            return usedRegisters;
        }
    }
}
