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

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
