namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.Assembly;
    using Compiler.Common;
    using Compiler.Optimization;
    using Compiler.SymbolTable;
    using Compiler.SyntaxTree;

    using Type = Compiler.Type;

    public static class MachineExpander
    {
        public static void ConvertToMachineDependant(ControlFlowGraph graph, SymbolTable symbolTable)
        {
            foreach (var function in graph.Functions)
            {
                AddParameters(graph, symbolTable, function);

                ConvertExponensiationToCalls(function, symbolTable);
                ConvertAllocToCalls(function, symbolTable);

                ConvertCallsToCallingConvention(symbolTable, function);
            }
        }

        private static void ConvertCallsToCallingConvention(SymbolTable symbolTable, KeyValuePair<string, IList<BasicBlock>> function)
        {
            int currentParam = 0;
            var callParameters = new List<VariableSymbol>();
            foreach (var statement in function.Value.SelectMany(m => m).ToArray())
            {
                var paramStatement = statement as ParamStatement;
                if (paramStatement != null)
                {
                    if (currentParam < CallingConvention.NumberOfRegisterParams)
                    {
                        var register = CallingConvention.GetArgumentRegister(paramStatement.Argument.Type, currentParam);

                        var registerVariable = MakeTempRegisterVariable(symbolTable, paramStatement.Argument.Type, register);
                        CFGUtilities.AddBefore(
                            paramStatement,
                            new AssignStatement(new VariableDestination(registerVariable), paramStatement.Argument));

                        CFGUtilities.RemoveStatement(paramStatement);

                        callParameters.Add(registerVariable);
                    }

                    currentParam++;
                }
                else if (statement is CallStatement)
                {
                    currentParam = 0;
                    var callStatement = (CallStatement)statement;
                    if (callStatement is ReturningCallStatement)
                    {
                        var returningCallStatement = callStatement as ReturningCallStatement;
                        var register = CallingConvention.GetReturnValueRegister(returningCallStatement.Return.Type);
                        var registerVariable = MakeTempRegisterVariable(symbolTable, returningCallStatement.Return.Type, register);

                        var assignStatement = new AssignStatement(
                            ((ReturningCallStatement)callStatement).Return,
                            new VariableArgument(registerVariable));

                        CFGUtilities.ReplaceStatement(
                            callStatement,
                            callStatement = new CallStatement(callStatement.Function, callStatement.NumberOfArguments));

                        CFGUtilities.AddAfter(callStatement, assignStatement);
                    }

                    callStatement.CallVariables.AddRange(callParameters);
                }
            }
        }

        private static void AddParameters(ControlFlowGraph graph, SymbolTable symbolTable, KeyValuePair<string, IList<BasicBlock>> function)
        {
            var numberOfRegisterParameters = Math.Min(
                graph.FunctionParameters[function.Key].Count,
                CallingConvention.NumberOfRegisterParams);

            for (int i = 0; i < numberOfRegisterParameters; i++)
            {
                var functionParameter = graph.FunctionParameters[function.Key][i];
                var register = CallingConvention.GetArgumentRegister(functionParameter.Type, i);

                var registerVariable = MakeTempRegisterVariable(symbolTable, functionParameter.Type, register);

                CFGUtilities.AddBefore(
                    function.Value.First().Enter,
                    new AssignStatement(new VariableDestination(functionParameter), new VariableArgument(registerVariable)));
            }
        }

        private static void ConvertExponensiationToCalls(
            KeyValuePair<string, IList<BasicBlock>> function,
            SymbolTable symbolTable)
        {
            foreach (var statement in function.Value.SelectMany(m => m).OfType<BinaryOperatorStatement>().ToArray())
            {
                if (statement.Operator == BinaryOperator.Exponensiation)
                {
                    CFGUtilities.AddBefore(statement, new ParamStatement(statement.Left));
                    CFGUtilities.AddBefore(statement, new ParamStatement(statement.Right));
                    CFGUtilities.ReplaceStatement(
                        statement,
                        new ReturningCallStatement(
                            (FunctionSymbol)symbolTable.GetSymbol("Power", SymbolType.Function),
                            2,
                            statement.Return));
                }
            }
        }

        private static void ConvertAllocToCalls(
            KeyValuePair<string, IList<BasicBlock>> function,
            SymbolTable symbolTable)
        {
            foreach (var statement in function.Value.SelectMany(m => m).OfType<AllocStatement>())
            {
                CFGUtilities.AddBefore(statement, new ParamStatement(statement.Size));
                CFGUtilities.ReplaceStatement(
                    statement,
                    new ReturningCallStatement(
                        (FunctionSymbol)symbolTable.GetSymbol("Alloc", SymbolType.Function),
                        1,
                        statement.Return));
            }
        }

        private static TempVariableSymbol MakeTempRegisterVariable(SymbolTable symbolTable, Type type, Register register)
        {
            var variable = new TempVariableSymbol(type) { Register = register };
            symbolTable.RegisterSymbol(variable);
            return variable;
        }
    }
}
