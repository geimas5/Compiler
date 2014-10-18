namespace Compiler.ControlFlowGraph
{
    using System.Collections;
    using System.Collections.Generic;

    public class BasicBlock : IEnumerable<Statement>
    {
        public BasicBlock(Statement enter, Statement exit)
        {
            this.Enter = enter;
            this.Exit = exit;

            var current = enter;
            current.BasicBlock = this;
            while ((current = current.Next) != null)
            {
                current.BasicBlock = this;

                if (current == exit)
                {
                    break;
                }
            }
        }

        public BasicBlock(Statement statement)
            : this(statement, statement)
        {   
        }

        public Statement Enter { get; set; }

        public Statement Exit { get; set; }

        public BasicBlock Append(Statement statement)
        {
            this.Exit.Next = statement;
            statement.Previous = this.Exit;
            statement.BasicBlock = this;

            return new BasicBlock(this.Enter, statement);
        }

        public BasicBlock Join(BasicBlock block)
        {
            this.Exit.Next = block.Enter;
            block.Enter.Previous = this.Exit;

            return new BasicBlock(this.Enter, block.Exit);
        }

        public IEnumerator<Statement> GetEnumerator()
        {
            var statement = this.Enter;
            while (statement != null && statement.BasicBlock == this)
            {
                yield return statement;

                statement = statement.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
