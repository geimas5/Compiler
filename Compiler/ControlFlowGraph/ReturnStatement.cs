namespace Compiler.ControlFlowGraph
{
    public class ReturnStatement : Statement
    {
        public ReturnStatement()
        {
            
        }

        public ReturnStatement(Argument value)
        {
            this.Value = value;
        }
        
        public Argument Value { get; private set; }

        public override string ToString()
        {
            if (Value == null)
            {
                return "Return";
            }
            else
            {
                return "Return " + Value;
            }
        }
    }
}
