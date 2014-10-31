namespace Compiler.ControlFlowGraph
{
    public class GlobalArgument : Argument
    {
        public GlobalArgument(string name)
            : base(Type.IntType)
        {
            this.Name = name;
        }

        public string Name { get; private set; }
    }
}
