namespace Compiler.Optimization
{
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.DataFlowAnalysis;
    using Compiler.SyntaxTree;

    public class DeadcodeEliminator : OptimizerBase
    {
        private IReadOnlyDictionary<BasicBlock, BlockLiveness> blockLiveness;

        protected override void Init(ControlFlowGraph graph)
        {
            this.blockLiveness = new LivenessAnalysis(graph).RunAnalysis();
        }

        public override void VisitBlock(BasicBlock block)
        {
            var livenessBlock = this.blockLiveness[block];

            foreach (var statement in livenessBlock.FindDeadStatements().Where(this.CanRemoveStatement).ToArray())
            {
                CFGUtilities.RemoveStatement(statement);
                this.SetSomethingChanged();
            }
        }

        private bool CanRemoveStatement(Statement statement)
        {
            var binaryOperatorStatement = statement as BinaryOperatorStatement;
            if (binaryOperatorStatement != null && binaryOperatorStatement.Operator == BinaryOperator.Divide)
            {
                //TODO: Check for divide by zero, for now, dont remove divide.
                return false;
            }

            if (statement is CallStatement)
            {
                return false;
            }

            return true;
        }
    }
}
