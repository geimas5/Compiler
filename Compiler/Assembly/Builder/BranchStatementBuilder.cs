namespace Compiler.Assembly.Builder
{
    using System;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    using Type = Compiler.Type;

    public class BranchStatementBuilder : StatementBuilder<BranchStatement>
    {
        protected override void Build()
        {
            if (Equals(this.Statement.Left.Type, Type.DoubleType))
            {
                this.WriteFloatingPointBranch();
            }
            else
            {
                this.WriteIntegerBranch();    
            }
        }

        private void WriteIntegerBranch()
        {
            var leftOperand = this.GetLeftOperand();
            var rightOperand = this.GetRightOperand(leftOperand is MemoryOperand, !(leftOperand is MemoryOperand));

            this.WriteBinaryInstruction(Opcode.CMP, leftOperand, rightOperand);

            JumpOpCodes opcode;

            switch (this.Statement.Operator)
            {
                case BinaryOperator.Less:
                    opcode = this.Statement.Zero ? JumpOpCodes.JGE : JumpOpCodes.JL;
                    break;
                case BinaryOperator.LessEqual:
                    opcode = this.Statement.Zero ? JumpOpCodes.JG : JumpOpCodes.JLE;
                    break;
                case BinaryOperator.Greater:
                    opcode = this.Statement.Zero ? JumpOpCodes.JLE : JumpOpCodes.JG;
                    break;
                case BinaryOperator.GreaterEqual:
                    opcode = this.Statement.Zero ? JumpOpCodes.JL : JumpOpCodes.JGE;
                    break;
                case BinaryOperator.Equal:
                    opcode = this.Statement.Zero ? JumpOpCodes.JNE : JumpOpCodes.JE;
                    break;
                case BinaryOperator.NotEqual:
                    opcode = this.Statement.Zero ? JumpOpCodes.JE : JumpOpCodes.JNE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            this.WriteInstruction(new JumpInstruction(opcode, "L" + this.Statement.BranchTarget.Id));
        }

        private Operand GetRightOperand(bool leftIsMemory, bool canBeImmediate = true)
        {
            var rightOperand = this.ArgumentToOperand(Statement.Right, Register.R11, Register.XMM14);

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
            var leftOperand = this.ArgumentToOperand(Statement.Left, Register.R10, Register.XMM14);
            if (leftOperand is ConstantOperand)
            {
                this.MoveData(leftOperand, new RegisterOperand(Register.R10), Register.R10, Register.XMM14);
                leftOperand = new RegisterOperand(Register.R10);
            }

            return leftOperand;
        }

        private void WriteFloatingPointBranch()
        {
            int comparisonType;

            switch (Statement.Operator)
            {
                case BinaryOperator.Equal:
                    comparisonType = (int)SDComparisonType.Equal;
                    break;
                case BinaryOperator.NotEqual:
                    comparisonType = (int)SDComparisonType.NotEqual;
                    break;
                case BinaryOperator.Less:
                    comparisonType = (int)SDComparisonType.LessThan;
                    break;
                case BinaryOperator.LessEqual:
                    comparisonType = (int)SDComparisonType.LessEqual;
                    break;
                case BinaryOperator.Greater:
                    comparisonType = (int)SDComparisonType.NotLessEqual;
                    break;
                case BinaryOperator.GreaterEqual:
                    comparisonType = (int)SDComparisonType.NotLessThan;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("statement", "Unsupported operator");
            }

            Operand leftOperand = new RegisterOperand(Register.XMM14);
            Operand rightOperand = new RegisterOperand(Register.XMM15);
            this.MoveData(this.ArgumentToOperand(Statement.Left, Register.R10, Register.XMM14), leftOperand, Register.R10, Register.XMM14);
            this.MoveData(this.ArgumentToOperand(Statement.Right, Register.R11, Register.XMM15), rightOperand, Register.R11, Register.XMM15);

            this.WriteTrinaryInstruction(TrinaryOpcode.CMPSD, leftOperand, rightOperand, new ConstantOperand(comparisonType));

            this.MoveData(leftOperand, new RegisterOperand(Register.R10), Register.R10, Register.XMM14);

            this.MoveData(new ConstantOperand(1), new RegisterOperand(Register.R11), Register.R11, Register.XMM15);
            this.WriteBinaryInstruction(Opcode.AND, new RegisterOperand(Register.R10), new RegisterOperand(Register.R11));

            this.WriteBinaryInstruction(Opcode.CMP, new RegisterOperand(Register.R10), new ConstantOperand(1));

            this.WriteInstruction(
                new JumpInstruction(
                    this.Statement.Zero ? JumpOpCodes.JNE : JumpOpCodes.JE,
                    "L" + this.Statement.BranchTarget.Id));
        }
    }
}
