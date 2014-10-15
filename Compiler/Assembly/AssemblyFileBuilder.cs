namespace Compiler.Assembly
{
    using System;
    using System.Collections.Generic;

    using Compiler.Common;
    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;
    using Compiler.SyntaxTree;

    using ReturnStatement = Compiler.ControlFlowGraph.ReturnStatement;

    public static class AssemblyFileBuilder
    {
        private static int currentParam = 0;

        private static int currentOffset = 0;

        public static void BuildFile(AssemblyFile file, ControlFlowGraph graph)
        {
            AddStrings(file, graph);
            AddFunctions(file, graph);
        }

        private static void AddStrings(AssemblyFile file, ControlFlowGraph graph)
        {
            foreach (var str in graph.Strings)
            {
                file.DataSection.DataEntries.Add(new StringEntry(str.Key, str.Value));
            }
        }

        private static void AddFunctions(AssemblyFile file, ControlFlowGraph graph)
        {
            foreach (var function in graph.Functions)
            {
                AddFunction(file, function.Key, function.Value);
            }
        }

        private static void AddFunction(
            AssemblyFile file,
            string name,
            IEnumerable<BasicBlock> functionBlocks)
        {
            if (name == "PrintLine")
            {
                name = "printf";
            }

            var procedure = new Procedure(name);

            var parameterOffsets = new Dictionary<string, int>();
            currentOffset = 0;

            foreach (var block in functionBlocks)
            {
                var assemblyBlock = new Block("L" + block.Enter.Id);
                procedure.Blocks.Add(assemblyBlock);

                foreach (var statement in block)
                {
                    assemblyBlock.Instructions.AddRange(CreateInstruction(statement, parameterOffsets));
                }
            }

            file.CodeSection.Procedures.Add(procedure);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            Statement statement,
            IDictionary<string, int> parameterOffsets)
        {
            IEnumerable<Instruction> instructions;

            if (statement is ParamStatement) instructions = CreateInstruction((ParamStatement)statement, parameterOffsets);
            else if (statement is CallStatement) instructions = CreateInstruction((CallStatement)statement, parameterOffsets);
            else if (statement is AssignStatement) instructions = CreateInstruction((AssignStatement)statement, parameterOffsets);
            else if (statement is ReturnStatement) instructions = CreateInstruction((ReturnStatement)statement, parameterOffsets);
            else if (statement is BinaryOperatorStatement) instructions = CreateInstruction((BinaryOperatorStatement)statement, parameterOffsets);
            else if (statement is JumpStatement) instructions = CreateInstruction((JumpStatement)statement, parameterOffsets);
            else if (statement is BranchStatement) instructions = CreateInstruction((BranchStatement)statement, parameterOffsets);
            else if (statement is NopStatement) instructions = new[] { new NOPInstruction() };
            else
            {
                throw new ArgumentException();
            }

            foreach (var instruction in instructions)
            {
                yield return instruction;
            }
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ParamStatement paramStatement,
            IDictionary<string, int> parameterOffsets)
        {
            Register paramReg;

            switch (currentParam++)
            {
                case 0:
                    paramReg = Register.RCX;
                    break;
                case 1:
                    paramReg = Register.RDX;
                    break;
                case 2:
                    paramReg = Register.R8;
                    break;
                case 3:
                    paramReg = Register.R9;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("currentParam");
            }

            yield return PlaceArgumentInRegister(paramReg, paramStatement.Argument, parameterOffsets);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            CallStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            currentParam = 0;

            string name = statement.Function.Name;

            if (name == "PrintLine")
            {
                name = "printf";
            }

            yield return new Instruction("call " + name);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ReturnStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            yield return PlaceArgumentInRegister(Register.RAX, statement.Value, parameterOffsets);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            AssignStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var destination = GetVariableDestination(statement.Return, parameterOffsets);

            yield return PlaceArgumentInRegister(Register.R10, statement.Argument, parameterOffsets);
            yield return new OpCodeInstruction(Opcode.MOV, destination, Register.R10.ToString());
        }

        private static IEnumerable<Instruction> CreateInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var opcode = ConvertToOpcode(statement.Operator);

            var destination = GetVariableDestination(statement.Return, parameterOffsets);

            yield return PlaceArgumentInRegister(Register.R10, statement.Left, parameterOffsets);
            yield return PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets);
            yield return new OpCodeInstruction(opcode, Register.R10.ToString(), Register.R11.ToString());
            yield return new OpCodeInstruction(Opcode.MOV, destination, Register.R10.ToString());
        }

        private static IEnumerable<Instruction> CreateInstruction(
            JumpStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            yield return new JumpInstruction("L" + statement.Target.Id);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            BranchStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            yield return PlaceArgumentInRegister(Register.R10, statement.Left, parameterOffsets);
            yield return PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets);
            yield return new OpCodeInstruction(Opcode.CMP, Register.R10.ToString(), Register.R11.ToString());

            SingleArgOpcode opcode;

            switch (statement.Operator)
            {
                case BinaryOperator.Less:
                    opcode = SingleArgOpcode.JL;
                    break;
                case BinaryOperator.LessEqual:
                    opcode = SingleArgOpcode.JLE;
                    break;
                case BinaryOperator.Greater:
                    opcode = SingleArgOpcode.JG;
                    break;
                case BinaryOperator.GreaterEqual:
                    opcode = SingleArgOpcode.JGE;
                    break;
                case BinaryOperator.Equal:
                    opcode = SingleArgOpcode.JE;
                    break;
                case BinaryOperator.NotEqual:
                    opcode = SingleArgOpcode.JNE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return new SingleOpcodeInstruction(opcode, "L" + statement.BranchTarget.Id);
        }

        private static Opcode ConvertToOpcode(BinaryOperator op)
        {
            switch (op)
            {
                case BinaryOperator.Add:
                    return Opcode.ADD;
                case BinaryOperator.Subtract:
                    return Opcode.SUB;
                case BinaryOperator.Multiply:
                    return Opcode.IMUL;
                case BinaryOperator.Divide:
                    return Opcode.IDIV;
                case BinaryOperator.Exponensiation:
                    break;
                case BinaryOperator.Mod:
                    break;
                case BinaryOperator.Less:
                    break;
                case BinaryOperator.LessEqual:
                    break;
                case BinaryOperator.Greater:
                    break;
                case BinaryOperator.GreaterEqual:
                    break;
                case BinaryOperator.And:
                    break;
                case BinaryOperator.Or:
                    break;
                case BinaryOperator.Equal:
                    break;
                case BinaryOperator.NotEqual:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new Exception();
        }

        private static Instruction PlaceArgumentInRegister(
            Register register,
            Argument argument,
            IDictionary<string, int> parameterOffsets)
        {
            var code = Opcode.MOV;
            string argument1 = register.ToString();
            string argument2;

            if (argument is GlobalArgument)
            {
                code = Opcode.LEA;
                argument2 = ((GlobalArgument)argument).Name;
            }
            else if (argument is IntConstantArgument)
            {
                argument2 = ((IntConstantArgument)argument).Value.ToString();
            }
            else if (argument is BooleanConstantArgument)
            {
                argument2 = Convert.ToInt32(((BooleanConstantArgument)argument).Value).ToString();
            }
            else if (argument is VariableArgument)
            {
                argument2 = GetVariableDestination(((VariableArgument)argument).Variable, parameterOffsets);
            }
            else
            {
                throw new Exception();
            }

            return new OpCodeInstruction(code, argument1, argument2);
        }

        private static string GetVariableDestination(
            VariableSymbol variableSymbol,
            IDictionary<string, int> parameterOffsets)
        {
            var variableOffset = GetVariableOffset(variableSymbol, parameterOffsets);

            return string.Format("[RBP {0}]", variableOffset);
        }

        private static int GetVariableOffset(
            VariableSymbol variableSymbol,
            IDictionary<string, int> parameterOffsets)
        {
            int varOffset;

            if (parameterOffsets.ContainsKey(variableSymbol.Name))
            {
                varOffset = parameterOffsets[variableSymbol.Name];
            }
            else
            {
                varOffset = currentOffset -= 4;
                parameterOffsets[variableSymbol.Name] = currentOffset;
            }

            return varOffset;
        }
    }
}
