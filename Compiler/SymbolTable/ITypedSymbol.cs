namespace Compiler.SymbolTable
{
    public interface ITypedSymbol : ISymbol
    {
        Type Type { get; }
    }
}
