namespace Compiler.Optimization.AlgeabraicRules
{
    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public  class IntMultiplyOneRule : IAlgeabraicRule
    {
        public bool ProcessStatement(Statement statement)
        {
            var binaryStatement = statement as BinaryOperatorStatement;
            if (binaryStatement == null)
            {
                return false;
            }

            if (binaryStatement.Operator != BinaryOperator.Multiply)
            {
                return false;
            }

            if (binaryStatement.Right is IntConstantArgument && ((IntConstantArgument)binaryStatement.Right).Value == 1)
            {
                CFGUtilities.ReplaceStatement(statement, new AssignStatement(binaryStatement.Return, binaryStatement.Left));
                return true;
            }

            if (binaryStatement.Left is IntConstantArgument && ((IntConstantArgument)binaryStatement.Left).Value == 1)
            {
                CFGUtilities.ReplaceStatement(statement, new AssignStatement(binaryStatement.Return, binaryStatement.Right));
                return true;
            }

            return false;
        }
    }
}
