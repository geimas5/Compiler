namespace Compiler.ControlFlowGraph
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Compiler.Assembly;
    using Compiler.Common;
    using Compiler.Optimization;
    using Compiler.SymbolTable;

    using Type = Compiler.Type;

    public static class MachineExpander
    {
        public static void ConvertToMachineDependant(ControlFlowGraph graph, SymbolTable symbolTable)
        {
            foreach (var function in graph.Functions)
            {
                for (int i = 0; i < Math.Min(graph.FunctionParameters[function.Key].Count, CallingConvention.NumberOfRegisterParams); i++)
                {
                    var functionParameter = graph.FunctionParameters[function.Key][i];
                    var register = CallingConvention.GetArgumentRegister(functionParameter.Type, i);

                    var registerVariable = MakeTempRegisterVariable(symbolTable, functionParameter.Type, register);

                    CFGUtilities.AddBefore(function.Value.First().Enter,
                            new AssignStatement(
                                new VariableDestination(functionParameter),
                                new VariableArgument(registerVariable)));
                }

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
                        callStatement.CallVariables.AddRange(callParameters);
                    }
                }
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
