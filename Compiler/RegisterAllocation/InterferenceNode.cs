namespace Compiler.RegisterAllocation
{
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.SymbolTable;

    public class InterferenceNode
    {
        public InterferenceNode(VariableSymbol variable)
        {
            this.Neighbours = new HashSet<InterferenceNode>();

            this.Variable = variable;
            
            if (variable.Register.HasValue)
            {
                IsPrecolored = true;
            }
        }

        public VariableSymbol Variable { get; private set; }


        public bool IsPrecolored { get; private set; }
        public HashSet<InterferenceNode> Neighbours { get; private set; }

        public bool IsRemoved { get; set; }

        public int Degree
        {
            get
            {
                return Neighbours.Count(m => !m.IsRemoved);
            }
        }
    }
}
