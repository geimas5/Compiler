namespace Compiler.SymbolTable
{
    using System;
    using System.Collections.Generic;

    public class SymbolTable
    {
        private readonly SymbolTable parent;

        private readonly Dictionary<string, ISymbol> variableSymbols = new Dictionary<string, ISymbol>();
        private readonly Dictionary<string, ISymbol> functionSymbols = new Dictionary<string, ISymbol>(); 

        private SymbolTable(SymbolTable parent)
        {
            this.parent = parent;
        }

        public SymbolTable()
        {
            
        }

        public bool IsRegisteredInScope(string name, SymbolType type)
        {
            switch (type)
            {
                case SymbolType.Variable:
                    return this.variableSymbols.ContainsKey(name);
                case SymbolType.Function:
                    return this.functionSymbols.ContainsKey(name);
            }

            return false;
        }

        public ISymbol GetSymbol(string name, SymbolType type)
        {
            if (type == SymbolType.Variable && this.variableSymbols.ContainsKey(name))
            {
                return this.variableSymbols[name];
            }
            
            if (type == SymbolType.Function && this.functionSymbols.ContainsKey(name))
            {
                return this.functionSymbols[name];
            }

            if (this.parent != null)
            {
                return this.parent.GetSymbol(name, type);
            }

            return null;
        }

        public void RegisterSymbol(ISymbol symbol)
        {
            if (symbol is VariableSymbol)
            {
                this.variableSymbols.Add(symbol.Name, symbol);
            }
            else if (symbol is FunctionSymbol)
            {
                this.functionSymbols.Add(symbol.Name, symbol);
            }
            else
            {
                throw new Exception("Symbol primitiveType is not supported in this symbol table");
            }
        }

        public SymbolTable CreateNestedSymbolTable()
        {
            return new SymbolTable(this);
        }
    }
}
