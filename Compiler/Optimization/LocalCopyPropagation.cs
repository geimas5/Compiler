namespace Compiler.Optimization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;

    public class LocalCopyPropagation : OptimizerBase
    {
        readonly Dictionary<VariableSymbol, Argument> values = new Dictionary<VariableSymbol, Argument>();

        public override void VisitBlock(BasicBlock block)
        {
            this.values.Clear();

            foreach (var statement in block.ToArray())
            {
                if (statement is BinaryOperatorStatement) this.HandleStatement((BinaryOperatorStatement)statement);
                else if (statement is ConvertToDoubleStatement) this.HandleStatement((ConvertToDoubleStatement)statement);
                else if (statement is ParamStatement) this.HandleStatement((ParamStatement)statement);
                else if (statement is ReturnStatement) this.HandleStatement((ReturnStatement)statement);
                else if (statement is AssignStatement) this.HandleStatement((AssignStatement)statement);
                else if (statement is BranchStatement) this.HandleStatement((BranchStatement)statement);
                else if (statement is IReturningStatement) this.HandleGenericReturningStatement((IReturningStatement)statement);
            }
        }

        private void HandleStatement(BinaryOperatorStatement statement)
        {
            var copyArg1 = this.GetCopy(statement.Left);
            var copyArg2 = this.GetCopy(statement.Right);

            if (copyArg1 == null && copyArg2 == null)
            {
                return;
            }

            var newOperatorStatement = new BinaryOperatorStatement(
                statement.Return,
                statement.Operator,
                copyArg1 ?? statement.Left,
                copyArg2 ?? statement.Right);

            CFGUtilities.ReplaceStatement(statement, newOperatorStatement);
            this.SetSomethingChanged();
        }

        private void HandleStatement(BranchStatement statement)
        {
            var copyArg1 = this.GetCopy(statement.Left);
            var copyArg2 = this.GetCopy(statement.Right);

            if (copyArg1 == null && copyArg2 == null)
            {
                return;
            }

            var newOperatorStatement = new BranchStatement(
                statement.Zero,
                statement.Operator,
                copyArg1 ?? statement.Left,
                copyArg2 ?? statement.Right,
                statement.BranchTarget);

            CFGUtilities.ReplaceStatement(statement, newOperatorStatement);
            this.SetSomethingChanged();
        }

        private void HandleStatement(ConvertToDoubleStatement statement)
        {
            var copyArg = this.GetCopy(statement.Argument);
            if (copyArg == null) return;

            var newStatement = new ConvertToDoubleStatement(statement.Return, copyArg);

            CFGUtilities.ReplaceStatement(statement, newStatement);
            this.SetSomethingChanged();
        }

        private void HandleStatement(ParamStatement statement)
        {
            var copyArg = this.GetCopy(statement.Argument);
            if (copyArg == null) return;

            var newStatement = new ParamStatement(copyArg);

            CFGUtilities.ReplaceStatement(statement, newStatement);
            this.SetSomethingChanged();
        }

        private void HandleStatement(ReturnStatement statement)
        {
            var copyArg = this.GetCopy(statement.Value);
            if (copyArg == null) return;

            var newStatement = new ReturnStatement(copyArg);

            CFGUtilities.ReplaceStatement(statement, newStatement);
            this.SetSomethingChanged();
        }

        private void HandleGenericReturningStatement(IReturningStatement statement)
        {
            this.values.Remove(statement.Return);
        }

        private void HandleStatement(AssignStatement statement)
        {
            this.values[statement.Return] = statement.Argument;
        }

        private Argument GetCopy(Argument argument)
        {
            var varArg = argument as VariableArgument;
            if (varArg == null) return null;

            if (this.values.ContainsKey(varArg.Variable))
            {
                return this.values[varArg.Variable];
            }

            return null;
        }
    }
}
