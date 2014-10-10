namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Text;

    public class IrPrinter
    {
        public void PrintIr(ControlFlowGraph graph)
        {
            var sb = new StringBuilder();

            foreach (var block in graph.Functions)
            {
                var statement = block.Enter;
                BasicBlock currentBlock = null;

                while (statement != null)
                {
                    if (currentBlock != statement.BasicBlock)
                    {
                        sb.AppendLine("-----------------");
                    }

                    currentBlock = statement.BasicBlock;

                    sb.AppendFormat("{0}: {1}", statement.Id, statement);
                    sb.AppendLine();

                    statement = statement.Next;
                }
            }

            Console.Write(sb);
        }
    }
}
