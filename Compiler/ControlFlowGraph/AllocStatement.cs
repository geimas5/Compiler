namespace Compiler.ControlFlowGraph
{
    public class AllocStatement : Statement, IReturningStatement
    {
        public AllocStatement(Destination destination, Argument size)
        {
            this.Size = size;
            this.Return = destination;
        }

        public Argument Size { get; private set; }

        public override string ToString()
        {
            return string.Format("{0} = Alloc({1})", this.Return, this.Size);
        }

        public Destination Return { get; private set; }
    }
}
