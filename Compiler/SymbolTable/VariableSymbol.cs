namespace Compiler.SymbolTable
{
    public class VariableSymbol : ITypedSymbol
    {
        public VariableSymbol(string name, Type type)
        {
            this.Type = type;
            this.Name = name;
        }

        public string Name { get; private set; }

        public Type Type { get; private set; }

        public override string ToString()
        {
            return Name; // + ":" + Type;
        }
    }
}
