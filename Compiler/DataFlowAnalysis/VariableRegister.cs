namespace Compiler.DataFlowAnalysis
{
    using System.Collections.Generic;

    using Compiler.SymbolTable;

    public class VariableRegister
    {
        readonly Dictionary<VariableSymbol, int> variables = new Dictionary<VariableSymbol, int>();
        readonly Dictionary<int, VariableSymbol> indexVariable = new Dictionary<int, VariableSymbol>();
        private int nextVariableIndex;

        public int GetVariableIndex(VariableSymbol variable)
        {
            if (this.variables.ContainsKey(variable))
            {
                return this.variables[variable];
            }

            this.indexVariable[this.nextVariableIndex] = variable;
            return this.variables[variable] = this.nextVariableIndex++;

        }

        public VariableSymbol GetVariable(int index)
        {
            return indexVariable[index];
        }

        public int Count
        {
            get
            {
                return this.nextVariableIndex;
            }
        }
    }
}
