namespace Compiler.Assembly.Builder
{
    using Compiler.ControlFlowGraph;

    public class CallStatementBuilder : StatementBuilder<CallStatement>
    {
        private readonly CallContext callContext;

        public CallStatementBuilder(CallContext callContext)
        {
            this.callContext = callContext;
        }

        protected override void Build()
        {
            if (this.callContext.Arguments.Count % 2 == 1)
            {
                this.WriteBinaryInstruction(Opcode.SUB, new RegisterOperand(Register.RSP), new ConstantOperand(8));
            }

            foreach (var callArgument in this.callContext.Arguments)
            {
                var operand = this.ArgumentToOperand(callArgument, Register.R10, Register.XMM14);
                this.WriteUnaryInstruction(SingleArgOpcode.PUSH, operand);
            }

            this.WriteBinaryInstruction(Opcode.SUB, new RegisterOperand(Register.RSP), new ConstantOperand(40));
            this.WriteInstruction(new CallInstruction(Statement.Function.Name));

            int stackFrameSize = 40 + (8 * this.callContext.Arguments.Count);

            if (this.callContext.Arguments.Count % 2 == 1) stackFrameSize += 8;

            this.WriteBinaryInstruction(Opcode.ADD, new RegisterOperand(Register.RSP), new ConstantOperand(stackFrameSize));

            callContext.Arguments.Clear();
        }
    }
}
