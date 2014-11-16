namespace Compiler.Assembly.Builder
{
    using System.Collections.Generic;

    using Compiler.ControlFlowGraph;

    public class CallContext
    {
        public CallContext()
        {
            Arguments = new List<Argument>();
        }

        public List<Argument> Arguments { get; private set; }
    }
}
