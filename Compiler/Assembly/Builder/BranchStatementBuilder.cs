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
            var rightOperand = this.GetRightOperand(leftOperand is MemoryOperand);

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

        private Operand GetRightOperand(bool leftIsMemory)
        {
            var variableArgument = Statement.Right as VariableArgument;
            if (variableArgument != null)
            {
                if (variableArgument.Variable.Register.HasValue)
                {
                    return new RegisterOperand(variableArgument.Variable.Register.Value);
                }

                var memoryOperand = this.Procedure.GetVarialeLocation(variableArgument.Variable);
                if (!leftIsMemory) return memoryOperand;

                this.WriteBinaryInstruction(Opcode.MOV, new RegisterOperand(Register.R11), memoryOperand);
                return new RegisterOperand(Register.R11);
            }

            var intConstantArgument = Statement.Right as IntConstantArgument;
            if (intConstantArgument != null)
            {
                if (leftIsMemory)
                {
                    this.PlaceArgumentInRegister(intConstantArgument, Register.R11);
                    return new RegisterOperand(Register.R11);
                }

                return new ConstantOperand(intConstantArgument.Value);
            }

            var globalConstantArgument = Statement.Right as GlobalArgument;
            if (globalConstantArgument != null)
            {
                this.PlaceArgumentInRegister(globalConstantArgument, Register.R11);
                return new RegisterOperand(Register.R11);
            }

            var pointerArgument = Statement.Right as PointerArgument;
            if (pointerArgument != null)
            {
                this.PlaceArgumentInRegister(pointerArgument, Register.R11);
                return new RegisterOperand(Register.R11);
            }

            throw new NotImplementedException();
        }

        private Operand GetLeftOperand()
        {
            var variableArgument = Statement.Left as VariableArgument;
            if (variableArgument != null)
            {
                if (variableArgument.Variable.Register.HasValue)
                {
                    return new RegisterOperand(variableArgument.Variable.Register.Value);
                }

                return Procedure.GetVarialeLocation(variableArgument.Variable);
            }

            this.PlaceArgumentInRegister(Statement.Left, Register.R10);

            return new RegisterOperand(Register.R10);
        }
    }
}
