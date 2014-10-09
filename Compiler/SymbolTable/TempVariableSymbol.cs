namespace Compiler.SymbolTable
{
    public class TempVariableSymbol : VariableSymbol
    {
        private static int currentIndex;

        public TempVariableSymbol(Type type)
            : base(string.Format("Temp-{0}", currentIndex++), type)
        {
        }
    }
}
