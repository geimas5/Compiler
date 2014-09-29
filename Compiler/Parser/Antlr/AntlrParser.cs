namespace Compiler.Parser.Antlr
{
    using Antlr4.Runtime;

    using Compiler.SyntaxTree;

    public class AntlrParser : IParser
    {
        private readonly Logger logger;

        public AntlrParser(Logger logger)
        {
            this.logger = logger;
        }

        public ParsingResult ParseProgram(string program)
        {
            var errorListener = new AntlrErrorListener(this.logger);

            var input = new AntlrInputStream(program);
            var lexer = new MLexer(input);
            lexer.RemoveErrorListeners();

            var tokens = new CommonTokenStream(lexer);

            var parser = new MParser(this.logger, tokens);
            parser.RemoveErrorListeners();
            lexer.AddErrorListener(errorListener);

            return new ParsingResult { SynataxTree = new SyntaxTree { RootNode = parser.program().programNode } };
        }
    }
}
