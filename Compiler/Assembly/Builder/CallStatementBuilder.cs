namespace Compiler.Assembly.Builder
{
    using Compiler.ControlFlowGraph;

    public class CallStatementBuilder : StatementBuilder<CallStatement>
    {
        protected override void Build()
        {

            this.WriteBinaryInstruction(Opcode.SUB, new RegisterOperand(Register.RSP), new ConstantOperand(40));
            this.WriteInstruction(new CallInstruction(Statement.Function.Name));
            this.WriteBinaryInstruction(Opcode.ADD, new RegisterOperand(Register.RSP), new ConstantOperand(40));
        }
    }
}
