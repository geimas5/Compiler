namespace Compiler.ControlFlowGraph
{
    using System.Collections.Generic;

    using Compiler.SymbolTable;

    public class CallStatement : Statement
    {
        public CallStatement(FunctionSymbol function, int numberOfArguments)
        {
            this.Function = function;
            this.NumberOfArguments = numberOfArguments;
            this.CallVariables = new List<VariableSymbol>();
        }

        public FunctionSymbol Function { get; set; }

        public int NumberOfArguments { get; set; }

        /// <summary>
        /// Gets or sets the call variables. Used to ensure variables after prepearing graph are contained in liveness analysis.
        /// </summary>
        /// <value>
        /// The call variables.
        /// </value>
        public IList<VariableSymbol> CallVariables { get; private set; }

        public override string ToString()
        {
            return "Call " + this.Function.Name + ", " + NumberOfArguments;
        }
    }
}
