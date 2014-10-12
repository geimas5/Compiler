namespace Compiler.ControlFlowGraph
{
    public class ParamStatement : Statement
    {
        public ParamStatement(Argument argument)
        {
            this.Argument = argument;
        }

        public Argument Argument { get; set; }

        public override string ToString()
        {
            return "Param " + Argument;
        }
    }
}
