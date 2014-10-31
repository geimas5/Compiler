namespace Compiler.DataFlowAnalysis
{
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;

    internal static class StatementHelper
    {
        public static  IEnumerable<VariableSymbol> GetStatementVariableUsages(Statement statement)
        {
            var symbols = new List<VariableSymbol>();

            var returningStatement = statement as IReturningStatement;
            if (returningStatement != null)
            {
                if (returningStatement.Return is PointerDestination)
                {
                    symbols.Add(((PointerDestination)returningStatement.Return).Destination);
                }
            }

            if (statement is BinaryOperatorStatement) symbols.AddRange(GetStatementVariableUsages((BinaryOperatorStatement)statement));
            if (statement is BranchStatement) symbols.AddRange(GetStatementVariableUsages((BranchStatement)statement));
            if (statement is ConvertToDoubleStatement) symbols.AddRange(GetStatementVariableUsages((ConvertToDoubleStatement)statement));
            if (statement is ParamStatement) symbols.AddRange(GetStatementVariableUsages((ParamStatement)statement));
            if (statement is UnaryOperatorStatement) symbols.AddRange(GetStatementVariableUsages((UnaryOperatorStatement)statement));
            if (statement is AssignStatement) symbols.AddRange(GetStatementVariableUsages((AssignStatement)statement));
            if (statement is ReturnStatement) symbols.AddRange(GetStatementVariableUsages((ReturnStatement)statement));
            if (statement is AllocStatement) symbols.AddRange(GetStatementVariableUsages((AllocStatement)statement));
            if (statement is CallStatement) symbols.AddRange(GetStatementVariableUsages((CallStatement)statement));

            if (statement is JumpStatement || statement is NopStatement)
            {
                return new VariableSymbol[0];
            }

            return symbols;
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(BinaryOperatorStatement statement)
        {
            return GetVariables(statement.Left, statement.Right);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(BranchStatement statement)
        {
            return GetVariables(statement.Left, statement.Right);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(ConvertToDoubleStatement statement)
        {
            return GetVariables(statement.Argument);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(ParamStatement statement)
        {
            return GetVariables(statement.Argument);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(UnaryOperatorStatement statement)
        {
            return GetVariables(statement.Argument);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(AssignStatement statement)
        {
            return GetVariables(statement.Argument);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(ReturnStatement statement)
        {
            return GetVariables(statement.Value);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(AllocStatement statement)
        {
            return GetVariables(statement.Size);
        }

        private static IEnumerable<VariableSymbol> GetStatementVariableUsages(CallStatement statement)
        {
            return statement.CallVariables;
        }

        private static IEnumerable<VariableSymbol> GetVariables(params Argument[] arguments)
        {
            return
                arguments.OfType<VariableArgument>()
                    .Select(argument => argument.Variable)
                    .Union(arguments.OfType<PointerArgument>().Select(argument => argument.Variable));
        }
    }
}
