namespace Compiler.SymbolTable
{
    public class ReturningFunctionSymbol : FunctionSymbol, ITypedSymbol
    {
        public ReturningFunctionSymbol(string name, Type type, string[] parameters)
            : base(name, parameters)
        {
            this.Type = type;
        }

        public Type Type { get; private set; }
    }
}
