namespace Compiler.Optimization
{
    using System.Linq;

    using Compiler.ControlFlowGraph;

    public class EqualAssignmentEliminator : OptimizerBase
    {
        public override bool RunOptimization(ControlFlowGraph graph)
        {
            bool somethingChanged = false;

            foreach (var block in graph.Functions.SelectMany(function => function.Value))
            {
                somethingChanged = this.ProcessBlock(block) || somethingChanged;
            }

            return somethingChanged;
        }

        private bool ProcessBlock(BasicBlock block)
        {
            if (block.Enter == block.Exit)
            {
                return false;
            }

            bool remove = true;
            bool somethingHasChanged = false;

            while (remove)
            {
                remove = false;
                Statement toRemove = null;

                foreach (var statement in block)
                {
                    var assignStatement = statement as AssignStatement;
                    if (assignStatement != null && (assignStatement.Argument is VariableArgument)
                        && ((VariableArgument)assignStatement.Argument).Variable == assignStatement.Return)
                    {
                        toRemove = statement;
                        remove = true;
                        somethingHasChanged = true;
                        break;
                    }
                }

                if (toRemove != null)
                {
                    this.RemoveStatement(toRemove);
                }
            }

            return somethingHasChanged;
        }

        private void RemoveStatement(Statement statement)
        {
            if (statement.Next != null)
            {
                statement.Next.Previous = statement.Previous;

                foreach (var jumpSource in statement.JumpSources.ToArray())
                {
                    if (jumpSource is BranchStatement)
                    {
                        ((BranchStatement)jumpSource).BranchTarget = statement.Next;
                        continue;
                    }

                    if (jumpSource is JumpStatement)
                    {
                        ((JumpStatement)jumpSource).Target = statement.Next;
                        continue;
                    }
                }
            }

            if (statement.Previous != null)
            {
                statement.Previous.Next = statement.Next;
            }

            if (statement.BasicBlock.Enter == statement)
            {
                statement.BasicBlock.Enter = statement.Next;
            }

            if (statement.BasicBlock.Exit == statement)
            {
                statement.BasicBlock.Exit = statement.Previous;
            }
        }
    }
}
