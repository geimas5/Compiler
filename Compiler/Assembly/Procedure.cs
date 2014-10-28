namespace Compiler.Assembly
{
    using System;
    using System.Collections.Generic;
    using System.IO;

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
            writer.WriteLine("mov rbp, rsp");
            writer.WriteLine("sub rsp, {0}", (16 * Math.Ceiling(((double)this.NumberOfLocalVariables * 8) / 16)) + 8); // Allocate stackframe space, and allign to 16 bit.

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
    }
}
