namespace Compiler.SemanticAnalysis
{
    using System.Linq;

    using Compiler.SyntaxTree;

    public class ReturnChecker : Visitor
    {
        private readonly Logger logger;

        public ReturnChecker(Logger logger)
        {
            this.logger = logger;
        }

        public void RunCheck(ProgramNode programNode)
        {
            programNode.Accept(this);
        }

        public override void Visit(ReturningFunctionDecleration node)
        {
            if (!node.Statements.Any(m => m is ReturnStatement))
            {
                logger.LogError(node.Location, "A returning function is required to have a "
                                               + "return statement as the first decendant of the method");
            }
        }
    }
}
