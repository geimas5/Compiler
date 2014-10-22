namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Text;

    using Compiler.DataFlowAnalysis;

    public class IrPrinter
    {
        public void PrintIr(ControlFlowGraph graph)
        {
            var sb = new StringBuilder();

            var analysis = new LivenessAnalysis(graph).RunAnalysis();

            foreach (var blocks in graph.Functions)
            {
                foreach (var block in blocks.Value)
                {
                    var liveBlock = analysis[block];

                    sb.AppendLine("-----------------");
                    sb.AppendLine("In=" + liveBlock.In + " out=" + liveBlock.Out);

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
