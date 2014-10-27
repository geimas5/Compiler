namespace Compiler.Optimization
{
    using System.Linq;
    using System.Runtime.Remoting.Messaging;

    using Compiler.ControlFlowGraph;

    public static class CFGUtilities
    {
        public static void RemoveStatement(Statement statement)
        {
            if (statement.Next != null)
            {
                statement.Next.Previous = statement.Previous;

                UpdateJumpSourcesToNewStatement(statement, statement.Next);
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

        public static void ReplaceStatement(Statement before, Statement after)
        {
            after.Next = before.Next;
            after.Previous = before.Previous;
            after.BasicBlock = before.BasicBlock;
            UpdateJumpSourcesToNewStatement(before, after);

            if (before.Previous != null) before.Previous.Next = after;
            if (before.Next != null) before.Next.Previous = after;

            if (before.BasicBlock.Enter == before) before.BasicBlock.Enter = after;
            if (before.BasicBlock.Exit == before) before.BasicBlock.Exit = after;
        }

        private static void UpdateJumpSourcesToNewStatement(Statement beforeStatement, Statement newStatement)
        {
            foreach (var jumpSource in beforeStatement.JumpSources.ToArray())
            {
                if (jumpSource is BranchStatement)
                {
                    ((BranchStatement)jumpSource).BranchTarget = newStatement;
                    continue;
                }

                if (jumpSource is JumpStatement)
                {
                    ((JumpStatement)jumpSource).Target = newStatement;
                    continue;
                }
            }
        }
    }
}
