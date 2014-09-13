namespace Compiler.SyntaxTree
{
    using System;
    using System.Collections.Generic;

    public class ProgramNode : Node
    {
        public ProgramNode(Location location)
            : base(location)
        {
            this.Functions = new List<FunctionDecleration>();
        }

        public List<FunctionDecleration> Functions { get; private set; }

        public override string ToString()
        {
            string text = "(Program " + this.Location + Environment.NewLine;

            foreach (var functionDecleration in this.Functions)
            {
                text += functionDecleration;
            }

            return text + ")";
        }
    }
}
