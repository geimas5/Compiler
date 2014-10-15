namespace Compiler.SymbolTable
{
    using System;
    using System.Linq;

    using Compiler.SyntaxTree;

    using Type = Compiler.Type;

    public static class SystemLibrary
    {
        public static void RegisterSystemFunctions(SymbolTable symbolTable)
        {
            RegisterFunction(symbolTable, "PrintLine", new Tuple<string, Type>("str", new Type(PrimitiveType.String)));
            RegisterFunction(symbolTable, "PrintInt", new Tuple<string, Type>("value", new Type(PrimitiveType.Int)));
            RegisterReturningFunction(
                symbolTable,
                "IntToString",
                new Type(PrimitiveType.Int),
                new Tuple<string, Type>("value", new Type(PrimitiveType.Int)));
        }

        private static void RegisterFunction(
            SymbolTable symbolTable,
            string functionName,
            params Tuple<string, Type>[] parameters)
        {
            var functionSymbol = new FunctionSymbol(functionName, parameters.Select(m => m.Item1).ToArray());

            functionSymbol.SymbolTable = symbolTable.CreateNestedSymbolTable();
            foreach (var parameter in parameters)
            {
                functionSymbol.SymbolTable.RegisterSymbol(new VariableSymbol(parameter.Item1, parameter.Item2));
            }

            symbolTable.RegisterSymbol(functionSymbol);
        }

        private static void RegisterReturningFunction(
            SymbolTable symbolTable,
            string functionName,
            Type type,
            params Tuple<string, Type>[] parameters)
        {
            var functionSymbol = new ReturningFunctionSymbol(functionName, type, parameters.Select(m => m.Item1).ToArray());

            functionSymbol.SymbolTable = symbolTable.CreateNestedSymbolTable();
            foreach (var parameter in parameters)
            {
                functionSymbol.SymbolTable.RegisterSymbol(new VariableSymbol(parameter.Item1, parameter.Item2));
            }

            symbolTable.RegisterSymbol(functionSymbol);
        }
    }
}
