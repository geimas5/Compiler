namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Text;

    public class IrPrinter
    {
        public void PrintIr(ControlFlowGraph graph)
        {
            var sb = new StringBuilder();

            foreach (var blocks in graph.Functions)
            {
                foreach (var block in blocks.Value)
                {
                    sb.AppendLine("-----------------");

                    foreach (var statement in block)
                    {
                        sb.AppendFormat("{0}: {1}", statement.Id, statement);
                        sb.AppendLine();
                    }   
                }
            }

            Console.Write(sb);
        }
    }
}
