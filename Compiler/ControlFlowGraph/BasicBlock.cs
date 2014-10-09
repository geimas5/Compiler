namespace Compiler.ControlFlowGraph
{
    public class BasicBlock
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

        public Statement Enter { get; private set; }

        public Statement Exit { get; private set; }

        public BasicBlock Append(Statement statement)
        {
            this.Exit.Next = statement;
            statement.Predecessors.Add(this.Exit);
            statement.BasicBlock = this;

            return new BasicBlock(this.Enter, statement);
        }

        public BasicBlock Join(BasicBlock block)
        {
            this.Exit.Next = block.Enter;
            block.Enter.Predecessors.Add(this.Exit);

            return new BasicBlock(this.Enter, block.Exit);
        }
    }
}
