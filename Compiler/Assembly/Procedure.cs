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

        public Procedure(string name, AssemblyFile assemblyFile)
        {
            this.Name = name;
            this.AssemblyFile = assemblyFile;
            this.Blocks = new List<Block>();
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
                writer.WriteLine("push {0}", usedRegister);
            }

            writer.WriteLine("mov rbp, rsp");
            writer.WriteLine("sub rsp, {0}", (16 * Math.Ceiling(((double)this.NumberOfLocalVariables * 8) / 16))); // Allocate stackframe space, and allign to 16 bit.

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
                writer.WriteLine("pop {0}", usedRegister);
            }
            

            writer.WriteLine("pop rbp");
            writer.WriteLine("ret");
            writer.WriteLine(this.Name + " ENDP");
        }

        public MemoryOperand GetVarialeLocation(VariableSymbol variableSymbol)
        {
            int varOffset;

            if (this.parameterOffsets.ContainsKey(variableSymbol.Name))
            {
                varOffset = this.parameterOffsets[variableSymbol.Name];
            }
            else
            {
                varOffset = currentOffset -= 8;
                this.parameterOffsets[variableSymbol.Name] = currentOffset;
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
