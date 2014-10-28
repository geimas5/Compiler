namespace Compiler.Optimization.AlgeabraicRules
{
    using System;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public class DoubleBinaryOperatorConstantRule : IAlgeabraicRule
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
            if (binaryOperator.Left is DoubleConstantArgument && binaryOperator.Right is DoubleConstantArgument)
            {
                switch (binaryOperator.Operator)
                {
                    case BinaryOperator.Add:
                    case BinaryOperator.Subtract:
                    case BinaryOperator.Multiply:
                    case BinaryOperator.Exponensiation:
                        return true;
                    case BinaryOperator.Divide:
                        return ((DoubleConstantArgument)binaryOperator.Right).Value != 0;
                    default:
                        return false;
                }
            }

            return false;
        }

        private void ReplaceStatement(BinaryOperatorStatement statement)
        {
            var leftValue = ((DoubleConstantArgument)statement.Left).Value;
            var rightValue = ((DoubleConstantArgument)statement.Right).Value;

            double calculatedValue;

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
                case BinaryOperator.Exponensiation:
                    calculatedValue = Math.Pow(leftValue, rightValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            CFGUtilities.ReplaceStatement(statement, new AssignStatement(statement.Return, new DoubleConstantArgument(calculatedValue)));
        }
    }
}
