namespace Compiler.Parser.Antlr
{
    using Antlr4.Runtime;

    using Compiler.SyntaxTree;

    public class AntlrParser : IParser
    {
        public ParsingResult ParseProgram(string program)
        {
            var input = new AntlrInputStream(program);
            var lexer = new MLexer(input);

            var tokens = new CommonTokenStream(lexer);

            var parser = new MParser(tokens);
            return new ParsingResult { SynataxTree = new SyntaxTree { RootNode = parser.program().programNode } };
        }
    }
}
