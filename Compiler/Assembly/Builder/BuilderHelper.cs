namespace Compiler.Assembly.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography;

    using Compiler.ControlFlowGraph;

    internal static class BuilderHelper
    {
        public static IEnumerable<Instruction> PlaceArgumentInRegister(
            Register register,
            Argument argument,
            Procedure currentProcedure)
        {
            var code = Opcode.MOV;
            var argument1 = new RegisterOperand(register);
            Operand argument2;

            if (argument is GlobalArgument)
            {
                code = Opcode.LEA;
                argument2 = new DataOperand(((GlobalArgument)argument).Name);
            }
            else if (argument is IntConstantArgument)
            {
                argument2 = new ConstantOperand(((IntConstantArgument)argument).Value);
            }
            else if (argument is BooleanConstantArgument)
            {
                argument2 = new ConstantOperand(Convert.ToInt32(((BooleanConstantArgument)argument).Value));
            }
            else if (argument is DoubleConstantArgument)
            {
                var value = ((DoubleConstantArgument)argument).Value;
                argument2 = new DataOperand(currentProcedure.AssemblyFile.DataSection.AddOrGetRealId(value));

                if (!RegisterUtility.IsXMM(register))
                {
                    return new[]
                               {
                                   new BinaryOpCodeInstruction(Opcode.MOVSD, new RegisterOperand(Register.XMM15), argument2),
                                   new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(register), new RegisterOperand(Register.XMM15))
                               };
                }

                return new[] { new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(register), argument2) };
            }
            else if (argument is VariableArgument)
            {
                var variableArgument = (VariableArgument)argument;

                if (variableArgument.Variable.Register.HasValue)
                {
                    return new[]
                               {
                                   new BinaryOpCodeInstruction(
                                       Opcode.MOV,
                                       argument1, 
                                       new RegisterOperand(variableArgument.Variable.Register.Value)),
                               };
                }

                argument2 = currentProcedure.GetVarialeLocation(variableArgument.Variable);

                if (RegisterUtility.IsXMM(register))
                {
                    return new[]
                               {
                                   new BinaryOpCodeInstruction(Opcode.MOV, new RegisterOperand(Register.R8), argument2),
                                   new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(register), new RegisterOperand(Register.R8))
                               };
                }

                return new[] { new BinaryOpCodeInstruction(code, argument1, argument2) };
            }
            else if (argument is PointerArgument)
            {
                var pointerArgument = (PointerArgument)argument;

                var instructions = new List<Instruction>();

                if (pointerArgument.Variable.Register.HasValue)
                {
                    instructions.Add(
                        new BinaryOpCodeInstruction(
                            Opcode.MOV,
                            new RegisterOperand(register),
                            new MemoryOperand(pointerArgument.Variable.Register.Value)));
                    return instructions;
                }
                
                instructions.Add(
                    new BinaryOpCodeInstruction(
                        Opcode.MOV,
                        new RegisterOperand(register),
                        currentProcedure.GetVarialeLocation(pointerArgument.Variable)));

                instructions.Add(new BinaryOpCodeInstruction(Opcode.MOV, new RegisterOperand(register), new MemoryOperand(register)));

                return instructions;
            }
            else
            {
                throw new Exception();
            }

            return new[] { new BinaryOpCodeInstruction(code, argument1, argument2) };
        }


        public static IEnumerable<Instruction> WriteRegisterToDestination(
            Destination destination,
            Register register,
            Procedure currentProcedure)
        {
            var instructions = new List<Instruction>();

            var variableDestination = destination as VariableDestination;
            if (variableDestination != null)
            {
                if (variableDestination.Variable.Register.HasValue)
                {
                    if (RegisterUtility.IsXMM(variableDestination.Variable.Register.Value) && !RegisterUtility.IsXMM(register)
                        || !RegisterUtility.IsXMM(variableDestination.Variable.Register.Value) && RegisterUtility.IsXMM(register))
                    {
                        instructions.Add(
                            new BinaryOpCodeInstruction(
                                Opcode.MOVD,
                                new RegisterOperand(variableDestination.Variable.Register.Value),
                                new RegisterOperand(register)));
                    }
                    else
                    {
                        instructions.Add(
                            new BinaryOpCodeInstruction(
                                Opcode.MOV,
                                new RegisterOperand(variableDestination.Variable.Register.Value),
                                new RegisterOperand(register)));
                    }
                }
                else
                {
                    var destinationOperand = currentProcedure.GetVarialeLocation(variableDestination.Variable);

                    instructions.Add(new BinaryOpCodeInstruction(Opcode.MOV, destinationOperand, new RegisterOperand(register)));
                }
            }
            else if (destination is PointerDestination)
            {
                var pointerDestination = (PointerDestination)destination;

                instructions.AddRange(PlaceArgumentInRegister(
                    Register.R11,
                    new VariableArgument(pointerDestination.Destination),
                    currentProcedure));

                instructions.Add(
                    new BinaryOpCodeInstruction(
                        Opcode.MOV,
                        new MemoryOperand(Register.R11),
                        new RegisterOperand(register)));
            }

            return instructions;
        }
    }
}
