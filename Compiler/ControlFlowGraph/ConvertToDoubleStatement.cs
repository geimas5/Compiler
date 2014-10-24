namespace Compiler.ControlFlowGraph
{
    public class ConvertToDoubleStatement : Statement, IReturningStatement
    {
        public ConvertToDoubleStatement(Destination @return, Argument argument)
        {
            this.Return = @return;
            this.Argument = argument;
        }

        public Destination Return { get; private set; }

        public Argument Argument { get; set; }

        public override string ToString()
        {
            return string.Format("{0} = ConvertToDouble({1})", this.Return, this.Argument);
        }
    }
}
