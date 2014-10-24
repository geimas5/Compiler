namespace Compiler.ControlFlowGraph
{
    using System;

    using Compiler.SyntaxTree;

    public class BinaryOperatorStatement : Statement, IReturningStatement
    {
        public BinaryOperatorStatement(Destination @return, BinaryOperator @operator, Argument left, Argument right)
        {
            this.Operator = @operator;
            this.Left = left;
            this.Right = right;
            this.Return = @return;
        }

        public BinaryOperator Operator { get; set; }

        public Argument Left { get; set; }

        public Argument Right { get; set; }

        public override string ToString()
        {
            string op = string.Empty;
            switch (this.Operator)
            {
                case BinaryOperator.Add:
                    op = "+";
                    break;
                case BinaryOperator.Subtract:
                    op = "-";
                    break;
                case BinaryOperator.Multiply:
                    op = "*";
                    break;
                case BinaryOperator.Divide:
                    op = "/";
                    break;
                case BinaryOperator.Exponensiation:
                    op = "**";
                    break;
                case BinaryOperator.Mod:
                    op = "%";
                    break;
                case BinaryOperator.Less:
                    op = "<";
                    break;
                case BinaryOperator.LessEqual:
                    op = "<=";
                    break;
                case BinaryOperator.Greater:
                    op = ">";
                    break;
                case BinaryOperator.GreaterEqual:
                    op = ">=";
                    break;
                case BinaryOperator.And:
                    op = "&&";
                    break;
                case BinaryOperator.Or:
                    op = "||";
                    break;
                case BinaryOperator.Equal:
                    op = "==";
                    break;
                case BinaryOperator.NotEqual:
                    op = "!=";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return string.Format("{0} = {1} {2} {3}", this.Return, this.Left, op, this.Right);
        }

        public Destination Return { get; private set; }
    }
}
