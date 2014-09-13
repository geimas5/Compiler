﻿namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class ReturningFunctionDecleration : FunctionDecleration
    {
        public ReturningFunctionDecleration(
            Location location,
            string name,
            IEnumerable<VariableNode> parameters,
            IEnumerable<StatementNode> statements,
            TypeNode type)
            : base(location, name, parameters, statements)
        {
            this.Type = type;
        }

        public TypeNode Type { get; set; }

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
