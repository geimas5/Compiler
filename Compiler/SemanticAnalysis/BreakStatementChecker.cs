namespace Compiler.SemanticAnalysis
{
    using Compiler.SyntaxTree;

    public class BreakStatementChecker : Visitor
    {
        private int loopLevel = 0;

        private readonly Logger logger;

        public BreakStatementChecker(Logger logger)
        {
            this.logger = logger;
        }

        public void RunCheck(ProgramNode node)
        {
            this.loopLevel = 0;

            node.Accept(this);
        }

        public override void Visit(ForStatement node)
        {
            loopLevel++;
            base.Visit(node);
            loopLevel--;
        }

        public override void Visit(WhileStatement node)
        {
            loopLevel++;
            base.Visit(node);
            loopLevel--;
        }

        public override void Visit(BreakStatement node)
        {
            if (loopLevel == 0)
            {
                this.logger.LogError(node.Location, "The break statement is not inside a loop");
            }
        }
    }
}
