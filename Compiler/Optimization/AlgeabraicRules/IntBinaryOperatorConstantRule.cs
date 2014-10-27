namespace Compiler.Optimization.AlgeabraicRules
{
    using System;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public class IntBinaryOperatorConstantRule : IAlgeabraicRule
    {
        public bool ProcessStatement(Statement statement)
        {
            var binaryOperator = statement as BinaryOperatorStatement;
            if (binaryOperator == null) return false;

            if (!IsMatch(binaryOperator)) return false;


            this.ReplaceStatement(binaryOperator);
            return true;
        }

        private static bool IsMatch(BinaryOperatorStatement binaryOperator)
        {
            if (binaryOperator.Left is IntConstantArgument && binaryOperator.Right is IntConstantArgument)
            {
                switch (binaryOperator.Operator)
                {
                    case BinaryOperator.Add:
                    case BinaryOperator.Subtract:
                    case BinaryOperator.Multiply:
                        return true;
                    case BinaryOperator.Divide:
                    case BinaryOperator.Mod:
                        return ((IntConstantArgument)binaryOperator.Right).Value != 0;
                    default:
                        return false;
                }
            }

            return false;
        }

        private void ReplaceStatement(BinaryOperatorStatement statement)
        {
            var leftValue = ((IntConstantArgument)statement.Left).Value;
            var rightValue = ((IntConstantArgument)statement.Right).Value;

            long calculatedValue;

            switch (statement.Operator)
            {
                case BinaryOperator.Add:
                    calculatedValue = leftValue + rightValue;
                    break;
                case BinaryOperator.Subtract:
                    calculatedValue = leftValue - rightValue;
                    break;
                case BinaryOperator.Multiply:
                    calculatedValue = leftValue * rightValue;
                    break;
                case BinaryOperator.Divide:
                    calculatedValue = leftValue / rightValue;
                    break;
                case BinaryOperator.Mod:
                    calculatedValue = leftValue % rightValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CFGUtilities.ReplaceStatement(statement, new AssignStatement(statement.Return, new IntConstantArgument(calculatedValue)));
        }
    }
}
