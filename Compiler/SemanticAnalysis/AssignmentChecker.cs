namespace Compiler.SemanticAnalysis
{
    using Compiler.SyntaxTree;

    public class AssignmentChecker : Visitor<bool>
    {
        private readonly Logger logger;

        public AssignmentChecker(Logger logger)
        {
            this.logger = logger;
        }

        public void RunCheck(ProgramNode programNode)
        {
            programNode.Accept(this);
        }

        public override bool Visit(AssignmentExpression node)
        {
            if (!node.LeftSide.Accept(this))
            {
                this.logger.LogError(node.Location, "Assigment can only be done to a valid location.");
            }

            return false;
        }

        public override bool Visit(VariableExpression node)
        {
            return node.VariableId.Accept(this);
        }

        public override bool Visit(VariableIdNode node)
        {
            return true;
        }

        public override bool Visit(IndexerExpression node)
        {
            return node.Name.Accept(this);
        }
    }
}
