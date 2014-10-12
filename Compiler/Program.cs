namespace Compiler
{
    using System;

    using Compiler.ControlFlowGraph;
    using Compiler.Parser.Antlr;
    using Compiler.SemanticAnalysis;
    using Compiler.SyntaxTree;

    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();

            var antlerParser = new AntlrParser(logger);

//            var result = antlerParser.ParseProgram(
//@"int main()
//{
//  int n = 43;
//  n = n * 3;
//  int f = n*n;
//  f = n / f;
//  return n;
//}");

            var result = antlerParser.ParseProgram(
@"int main()
{
    PrintLine(""Test"");
    int i;
    int j;

    for(i = 0; i < 5; i = i +1){
        for(j = 0; j < 5; j = j +1) {
              PrintLine(IntToString(i + j));
        }
    }

   return 0;
}
");

            var printer = new TreePrinter();
            printer.PrintTree(result.SynataxTree);
            
            var semanticChecker = new SemanticChecker(logger);
            semanticChecker.RunCheck(result.SynataxTree);

            logger.PrintMessages();

            if (logger.TotalErrors > 0)
            {
                Console.WriteLine(
                    "Compilation failed, {0} errors {1} warnings, and {2} information",
                    logger.TotalErrors,
                    logger.TotalWarnings,
                    logger.TotalInfo);

                Console.ReadLine();
                return;
            }

            var builder = new ControlFlowGraphBuilder();
            var controlGraph = builder.BuildGraph(result.SynataxTree.RootNode);

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Ir:");
            Console.WriteLine();

            new IrPrinter().PrintIr(controlGraph);

            Console.ReadLine();

        }

    }

}
