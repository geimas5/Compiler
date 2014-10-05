namespace Compiler.Parser.Antlr
{
    using Antlr4.Runtime;

    using Compiler.SyntaxTree;

    public class AntlrErrorListener : IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
    {
        private readonly Logger logger;

        public AntlrErrorListener(Logger logger)
        {
            this.logger = logger;
        }

        public void SyntaxError(
            IRecognizer recognizer,
            int offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            this.logger.LogError(new Location(charPositionInLine, line), msg);
        }

        public void SyntaxError(
            IRecognizer recognizer,
            IToken offendingSymbol,
            int line,
            int charPositionInLine,
            string msg,
            RecognitionException e)
        {
            this.logger.LogError(new Location(charPositionInLine, line), msg);
        }
    }
}
