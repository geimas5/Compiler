namespace Compiler
{
    using System;
    using System.IO;

    using Compiler.Assembly;
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
    if(1==2)
        PrintLine(""Test1"");
    else
       PrintLine(""Test2"");

    return 0;
}");

            var printer = new TreePrinter();
            printer.PrintTree(result.SynataxTree);
            
            var semanticChecker = new SemanticChecker(logger);
            var symbolTable = semanticChecker.RunCheck(result.SynataxTree);

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

            using (var stream = File.Create("output.asm"))
            using (var writer = new StreamWriter(stream))
            {
                var assemblyFile = new AssemblyFile();
                AssemblyFileBuilder.BuildFile(assemblyFile, controlGraph);

                assemblyFile.Write(writer);
            }

            //string d = "set LINKCMD64=C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\bin\amd64\link.exe";
            //var assebleBat = @"""C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\bin\amd64\ml64"" output.asm /link /subsystem:console /defaultlib:""C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Lib\x64\Kernel32.Lib"" /entry:main";
            //File.WriteAllText("Assemble.bat", assebleBat);

            // ml64 output.asm /link /subsystem:console /defaultlib:"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Lib\x64\Kernel32.Lib" /entry:main

            Console.ReadLine();
        }
    }
}
