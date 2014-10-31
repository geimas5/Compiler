namespace Compiler.ControlFlowGraph
{
    public abstract class Argument
    {
        protected Argument(Type type)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }
    }
}
