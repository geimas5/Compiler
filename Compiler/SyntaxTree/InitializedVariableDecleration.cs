﻿namespace Compiler.SyntaxTree
{
    public class InitializedVariableDecleration : VariableDecleration
    {
        public InitializedVariableDecleration(Location location, VariableNode variable, ExpressionNode initialization)
            : base(location, variable)
        {
            this.Initialization = initialization;
        }

        public ExpressionNode Initialization { get; set; }

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
