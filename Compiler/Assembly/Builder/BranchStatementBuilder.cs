namespace Compiler.Assembly.Builder
{
    using System;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public class BranchStatementBuilder : StatementBuilder<BranchStatement>
    {
        protected override void Build()
        {
            var leftOperand = this.GetLeftOperand();
            var rightOperand = this.GetRightOperand(leftOperand is MemoryOperand, !(leftOperand is MemoryOperand));

            this.WriteBinaryInstruction(Opcode.CMP, leftOperand, rightOperand);

            JumpOpCodes opcode;

            switch (Statement.Operator)
            {
                case BinaryOperator.Less:
                    opcode = Statement.Zero ? JumpOpCodes.JGE : JumpOpCodes.JL;
                    break;
                case BinaryOperator.LessEqual:
                    opcode = Statement.Zero ? JumpOpCodes.JG : JumpOpCodes.JLE;
                    break;
                case BinaryOperator.Greater:
                    opcode = Statement.Zero ? JumpOpCodes.JL : JumpOpCodes.JGE;
                    break;
                case BinaryOperator.GreaterEqual:
                    opcode = Statement.Zero ? JumpOpCodes.JL : JumpOpCodes.JGE;
                    break;
                case BinaryOperator.Equal:
                    opcode = Statement.Zero ? JumpOpCodes.JNE : JumpOpCodes.JE;
                    break;
                case BinaryOperator.NotEqual:
                    opcode = Statement.Zero ? JumpOpCodes.JE : JumpOpCodes.JNE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.WriteInstruction(new JumpInstruction(opcode, "L" + Statement.BranchTarget.Id));
        }

        private Operand GetRightOperand(bool leftIsMemory, bool canBeImmediate = true)
        {
            var rightOperand = this.ArgumentToOperand(Statement.Right, Register.R11);

            if (leftIsMemory && rightOperand is MemoryOperand)
            {
                this.MoveData(rightOperand, new RegisterOperand(Register.R11), Register.R11, Register.XMM14);

                rightOperand = new RegisterOperand(Register.R11);
            }

            if (!canBeImmediate && rightOperand is ConstantOperand)
            {
                this.MoveData(rightOperand, new RegisterOperand(Register.R11), Register.R11, Register.XMM14);

                rightOperand = new RegisterOperand(Register.R11);
            }

            return rightOperand;
        }

        private Operand GetLeftOperand()
        {
            var leftOperand = this.ArgumentToOperand(Statement.Left, Register.R10);
            if (leftOperand is ConstantOperand)
            {
                this.MoveData(leftOperand, new RegisterOperand(Register.R10), Register.R10, Register.XMM14);
                leftOperand = new RegisterOperand(Register.R10);
            }

            return leftOperand;
        }
    }
}
