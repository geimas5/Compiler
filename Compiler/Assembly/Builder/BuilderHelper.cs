namespace Compiler.Assembly.Builder
{
    using System;
    using System.Collections.Generic;

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
                                   new BinaryOpCodeInstruction(Opcode.MOVSD, new RegisterOperand(Register.XMM2), argument2),
                                   new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(register), new RegisterOperand(Register.XMM2))
                               };
                }

                return new[] { new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(register), argument2) };
            }
            else if (argument is VariableArgument)
            {
                argument2 = currentProcedure.GetVarialeLocation(((VariableArgument)argument).Variable);

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
                var destinationOperand = currentProcedure.GetVarialeLocation(variableDestination.Variable);

                instructions.Add(new BinaryOpCodeInstruction(Opcode.MOV, destinationOperand, new RegisterOperand(register)));
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
