namespace Compiler.Parser.Antlr
{
    using Antlr4.Runtime;

    using Compiler.SyntaxTree;

    public static class Utility
    {
        public static Location CreateLocation(ParserRuleContext context)
        {
            return new Location(context.Start.Column, context.Start.Line);
        }

        public static Location CreateLocation(IToken token)
        {
            return new Location(token.Column, token.Line);
        }
    }
}
