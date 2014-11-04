namespace Compiler.Assembly.Builder
{
    using System;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public sealed class UnaryOperatorBuilder : StatementBuilder<UnaryOperatorStatement>
    {
        protected override void Build()
        {
            switch (Statement.Operator)
            {
                case UnaryOperator.Not:
                    this.WriteNot();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void WriteNot()
        {
            this.MoveData(
                this.ArgumentToOperand(Statement.Argument, Register.R10, Register.XMM14),
                new RegisterOperand(Register.R10),
                Register.R10,
                Register.XMM14);

            this.WriteBinaryInstruction(Opcode.XOR, new RegisterOperand(Register.R10), new ConstantOperand(1));

            this.MoveData(
                new RegisterOperand(Register.R10),
                this.DestinationToOperand(Statement.Return, Register.R10),
                Register.R10,
                Register.XMM14);
        }
    }
}
