namespace Compiler.Optimization.AlgeabraicRules
{
    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public class IntMultiplyZeroRule : IAlgeabraicRule
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

            if (binaryStatement.Right is IntConstantArgument && ((IntConstantArgument)binaryStatement.Right).Value == 0)
            {
                CFGUtilities.ReplaceStatement(statement, new AssignStatement(binaryStatement.Return, new IntConstantArgument(0)));
                return true;
            }

            if (binaryStatement.Left is IntConstantArgument && ((IntConstantArgument)binaryStatement.Left).Value == 0)
            {
                CFGUtilities.ReplaceStatement(statement, new AssignStatement(binaryStatement.Return, new IntConstantArgument(0)));
                return true;
            }

            return false;
        }
    }
}
