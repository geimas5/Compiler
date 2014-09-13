﻿namespace Compiler.SyntaxTree
{
    public class VariableExpression : ExpressionNode
    {
        public VariableExpression(Location location, VariableIdNode variableId)
            : base(location)
        {
            this.VariableId = variableId;
        }

        public VariableIdNode VariableId { get; set; }

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
