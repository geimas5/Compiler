﻿namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class VoidFunctionDecleration : FunctionDecleration
    {
        public VoidFunctionDecleration(
            Location location,
            string name,
            IEnumerable<VariableNode> parameters,
            IEnumerable<StatementNode> statements)
            : base(location, name, parameters, statements)
        {
        }

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
