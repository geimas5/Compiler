namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Text;

    using Compiler.DataFlowAnalysis;

    public class IrPrinter
    {
        public string PrintIr(ControlFlowGraph graph)
        {
            var sb = new StringBuilder();

            var analysis = new LivenessAnalysis(graph).RunAnalysis();

            foreach (var function in graph.Functions)
            {
                sb.AppendLine("--------" + function.Key + "--------");

                foreach (var block in function.Value)
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
            return sb.ToString();
        }
    }
}
