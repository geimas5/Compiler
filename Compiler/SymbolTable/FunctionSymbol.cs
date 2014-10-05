namespace Compiler.SymbolTable
{
    public class FunctionSymbol : ISymbol
    {
        public FunctionSymbol(string name, string[] parameters)
        {
            this.Parameters = parameters;
            this.Name = name;
        }

        public string Name { get; private set; }

        public string[] Parameters { get; private set; }

        public SymbolTable SymbolTable { get; set; }
    }
}
