namespace Compiler.ControlFlowGraph
{
    public class AssignStatement : Statement, IReturningStatement
    {
        public AssignStatement(Destination destination, Argument argument)
        {
            this.Argument = argument;
            this.Return = destination;
        }

        public Argument Argument { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} = {1}", this.Return, Argument);
        }

        public Destination Return { get; private set; }
    }
}
