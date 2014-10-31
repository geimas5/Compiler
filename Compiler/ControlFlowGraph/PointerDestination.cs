namespace Compiler.ControlFlowGraph
{
    using Compiler.SymbolTable;

    public class PointerDestination : Destination
    {
        private readonly Type type;

        public PointerDestination(VariableSymbol destination, Type type)
        {
            this.Destination = destination;
            this.type = type;
        }

        public VariableSymbol Destination { get; private set; }

        public override Type Type
        {
            get
            {
                return this.type;
            }
        }

        public override string ToString()
        {
            if (Destination.Register.HasValue)
            {
                return string.Format("*({0})", this.Destination.Register);    
            }

            return string.Format("*({0})", this.Destination);    
        }
    }
}
