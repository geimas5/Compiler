namespace Compiler
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Compiler.Assembly;
    using Compiler.ControlFlowGraph;
    using Compiler.DataFlowAnalysis;
    using Compiler.Optimization;
    using Compiler.Parser.Antlr;
    using Compiler.SemanticAnalysis;
    using Compiler.SyntaxTree;

    public class CompilerAssembly
    {
        private readonly Optimizer optimizer;

        public CompilerAssembly()
        {
            this.Logger = new Logger();
            this.optimizer = new Optimizer();
        }

        public Logger Logger { get; private set; }

        public bool PrintTree { get; set; }

        public bool PrintIR { get; set; }

        public bool PrintMessages { get; set; }

        public IList<Optimizations> ActivatedOptimizations
        {
            get
            {
                return this.optimizer.ActivatedOptimizations;
            }
        }

        public bool CompileProgram(TextReader input, TextWriter output)
        {
            var antlerParser = new AntlrParser(this.Logger);

            var result = antlerParser.ParseProgram(input.ReadToEnd());

            if (this.PrintTree) this.PrintSyntaxTree(result.SynataxTree);

            this.RunSemanticCheck(result.SynataxTree);

            if (this.PrintMessages) this.Logger.PrintMessages();

            if (Logger.TotalErrors > 0)
            {
                return false;
            }

            var builder = new ControlFlowGraphBuilder();
            var controlGraph = builder.BuildGraph(result.SynataxTree.RootNode);

            this.optimizer.RunOptimizations(controlGraph);

            if (this.PrintIR) this.PrintIrCode(controlGraph);

            var assemblyFile = new AssemblyFile();
            AssemblyFileBuilder.BuildFile(assemblyFile, controlGraph);

            assemblyFile.Write(output);

            return true;
        }

        private void PrintSyntaxTree(SyntaxTree.SyntaxTree syntaxTree)
        {
            var printer = new TreePrinter();
            printer.PrintTree(syntaxTree);
        }

        private void PrintIrCode(ControlFlowGraph.ControlFlowGraph controlFlowGraph)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Ir:");
            Console.WriteLine();

            new IrPrinter().PrintIr(controlFlowGraph);
        }

        private void RunSemanticCheck(SyntaxTree.SyntaxTree syntaxTree)
        {
            var semanticChecker = new SemanticChecker(Logger);
            semanticChecker.RunCheck(syntaxTree);
        }
    }
}
