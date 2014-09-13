namespace Compiler.SyntaxTree
{
    using System.Collections.Generic;

    public class FunctionCallExpression : ExpressionNode
    {
        public FunctionCallExpression(Location location, string name, IEnumerable<ExpressionNode> arguments)
            : base(location)
        {
            this.Arguments = new List<ExpressionNode>();

            this.Name = name;
            this.Arguments.AddRange(arguments);
        }

        public string Name { get; set; }

        public List<ExpressionNode> Arguments { get; set; }
    }
}
