namespace Compiler.ControlFlowGraph
{
    using System;

    public class IrPrinter
    {
        public void PrintIr(ControlFlowGraph graph)
        {
            foreach (var block in graph.Functions)
            {
                Statement statement = block.Enter;

                while (statement != null)
                {
                    Console.WriteLine("{0}: {1}", statement.Id, statement);

                    statement = statement.Next;
                }
            }
        }
    }
}
