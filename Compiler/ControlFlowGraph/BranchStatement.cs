namespace Compiler.ControlFlowGraph
{
    using System;

    using Compiler.SyntaxTree;

    public class BranchStatement : Statement
    {
        private Statement branchTarget;

        public BranchStatement(bool zero, BinaryOperator @operator, Argument left, Argument right, Statement branchTarget)
        {
            this.BranchTarget = branchTarget;
            this.Operator = @operator;
            this.Left = left;
            this.Right = right;
            this.Zero = zero;
        }

        public bool Zero { get; set; }

        public BinaryOperator Operator { get; private set; }

        public Argument Left { get; private set; }

        public Argument Right { get; private set; }

        public Statement BranchTarget
        {
            get
            {
                return this.branchTarget;
            }
            set
            {
                if (this.branchTarget != null)
                {
                    this.branchTarget.JumpSources.Remove(this);
                }

                this.branchTarget = value;
                this.branchTarget.JumpSources.Add(this);
            }
        }

        public override string ToString()
        {
            string op = string.Empty;
            switch (Operator)
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

            return string.Format(
                "If{4} {0} {1} {2}, Goto {3}",
                this.Left,
                op,
                this.Right,
                this.BranchTarget.Id,
                this.Zero ? "Z" : string.Empty);
        }
    }
}
