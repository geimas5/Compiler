namespace Compiler.Parser.Antlr
{
    using Antlr4.Runtime;

    public partial class MParser
    {
        private readonly NodeFactory nodeFactory;
        private readonly Logger logger;

        public MParser(Logger logger, ITokenStream stream)
            : this(stream)
        {
            this.logger = logger;
            this.nodeFactory = new NodeFactory(logger);
        }
    }
}
