namespace Compiler.SemanticAnalysis
{
    using Compiler.SyntaxTree;

    public class SemanticChecker
    {
        private readonly Logger logger;

        public SemanticChecker(Logger logger)
        {
            this.logger = logger;
        }

        public void RunCheck(SyntaxTree syntaxTree)
        {
            var symbolTableBuilder = new SymbolTableBuilder(this.logger);
            symbolTableBuilder.BuildSymbolTable(syntaxTree.RootNode);

            var breakStatementChecker = new BreakStatementChecker(this.logger);
            breakStatementChecker.RunCheck(syntaxTree.RootNode);

            var typeChecker = new TypeChecker(this.logger);
            typeChecker.RunCheck(syntaxTree.RootNode);

            var returnChecker = new ReturnChecker(this.logger);
            returnChecker.RunCheck(syntaxTree.RootNode);

            var assignmentChecker = new AssignmentChecker(this.logger);
            assignmentChecker.RunCheck(syntaxTree.RootNode);
        }
    }
}
