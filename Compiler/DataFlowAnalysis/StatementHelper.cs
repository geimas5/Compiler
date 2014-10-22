namespace Compiler.DataFlowAnalysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;

    internal static class StatementHelper
    {
        public static  IEnumerable<VariableSymbol> GetStatementVariableUsages(Statement statement)
        {
            if (statement is BinaryOperatorStatement) return GetStatementVariableUsages((BinaryOperatorStatement)statement);
            if (statement is BranchStatement) return GetStatementVariableUsages((BranchStatement)statement);
            if (statement is ConvertToDoubleStatement) return GetStatementVariableUsages((ConvertToDoubleStatement)statement);
            if (statement is ParamStatement) return GetStatementVariableUsages((ParamStatement)statement);
            if (statement is UnaryOperatorStatement) return GetStatementVariableUsages((UnaryOperatorStatement)statement);
            if (statement is AssignStatement) return GetStatementVariableUsages((AssignStatement)statement);
            if (statement is ReturnStatement) return GetStatementVariableUsages((ReturnStatement)statement);

            if (statement is CallStatement || statement is JumpStatement || statement is NopStatement)
            {
                return new VariableSymbol[0];
            }

            throw new ArgumentException("The statement type is not supported", "statement");
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

        private static IEnumerable<VariableSymbol> GetVariables(params Argument[] arguments)
        {
            return arguments.OfType<VariableArgument>().Select(argument => argument.Variable);
        }
    }
}
