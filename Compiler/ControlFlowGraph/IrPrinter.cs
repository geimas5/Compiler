namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Linq;

    public class IrPrinter
    {
        public void PrintIr(ControlFlowGraph graph)
        {
            foreach (var block in graph.Functions)
            {
                var statement = block.First().Enter;
                BasicBlock currentBlock = null;

                while (statement != null)
                {
                    if (currentBlock != statement.BasicBlock)
                    {
                        Console.WriteLine("-----------------");
                    }

                    currentBlock = statement.BasicBlock;

                    Console.WriteLine("{0}: {1}", statement.Id, statement);

                    statement = statement.Next;
                }
            }
        }
    }
}
