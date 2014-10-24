
namespace Compiler.Optimization
{
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    public class AlgebraicOptimization : OptimizerBase
    {
        public override void VisitBlock(BasicBlock block)
        {
            foreach (var statement in block.ToArray())
            {
                this.ProcessStatement(statement);
            }
        }

        private void ProcessStatement(Statement statement)
        {
            if (statement is BinaryOperatorStatement) this.ProcessStatement((BinaryOperatorStatement)statement);
        }

        private void ProcessStatement(BinaryOperatorStatement statement)
        {
            if (statement.Left is IntConstantArgument && statement.Right is IntConstantArgument)
            {
                this.ProcessIntBinaryOperator(statement);
            }
            else if (statement.Right is IntConstantArgument && ((IntConstantArgument)statement.Right).Value == 0 && statement.Operator == BinaryOperator.Add)
            {
                CFGUtilities.ReplaceStatement(statement, new AssignStatement(statement.Return, statement.Left));
                this.SetSomethingChanged();
            }
            else if (statement.Left is IntConstantArgument && ((IntConstantArgument)statement.Left).Value == 0 && statement.Operator == BinaryOperator.Add)
            {
                CFGUtilities.ReplaceStatement(statement, new AssignStatement(statement.Return, statement.Right));
                this.SetSomethingChanged();
            }
        }

        private void ProcessIntBinaryOperator(BinaryOperatorStatement statement)
        {
            long newValue;

            switch (statement.Operator)
            {
                case BinaryOperator.Add:
                    newValue = ((IntConstantArgument)statement.Left).Value + ((IntConstantArgument)statement.Right).Value;
                    break;
                case BinaryOperator.Subtract:
                    newValue = ((IntConstantArgument)statement.Left).Value - ((IntConstantArgument)statement.Right).Value;
                    break;
                case BinaryOperator.Multiply:
                    newValue = ((IntConstantArgument)statement.Left).Value * ((IntConstantArgument)statement.Right).Value;
                    break;
                case BinaryOperator.Divide:
                    if (((IntConstantArgument)statement.Right).Value == 0)
                    {
                        return;
                    }

                    newValue = ((IntConstantArgument)statement.Left).Value / ((IntConstantArgument)statement.Right).Value;
                    break;
                case BinaryOperator.Mod:
                    if (((IntConstantArgument)statement.Right).Value == 0)
                    {
                        return;
                    }

                    newValue = ((IntConstantArgument)statement.Left).Value % ((IntConstantArgument)statement.Right).Value;
                    break;
                default:
                    return;
            }

            CFGUtilities.ReplaceStatement(statement, new AssignStatement(statement.Return, new IntConstantArgument(newValue)));
            this.SetSomethingChanged();
        }
    }
}
