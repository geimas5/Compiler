namespace Compiler.Assembly.Builder
{
    using Compiler.ControlFlowGraph;

    public sealed class AssignStatementBuilder : StatementBuilder<AssignStatement>
    {
        protected override void Build()
        {
            var destination = this.DestinationToOperand(Statement.Return, Register.R10);
            var argument = this.ArgumentToOperand(Statement.Argument, Register.R11, Register.XMM14);

            this.MoveData(argument, destination, Register.R11, Register.XMM14);
        }
    }
}
