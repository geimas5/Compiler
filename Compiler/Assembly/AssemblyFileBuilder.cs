namespace Compiler.Assembly
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Compiler.Common;
    using Compiler.ControlFlowGraph;
    using Compiler.SymbolTable;
    using Compiler.SyntaxTree;

    using ReturnStatement = Compiler.ControlFlowGraph.ReturnStatement;
    using Type = Compiler.Type;

    public static class AssemblyFileBuilder
    {
        private static int currentParam = 0;

        private static int currentOffset = 0;

        private static string currentFunction = string.Empty;

        private static ControlFlowGraph Graph;

        private static List<double> Reals = new List<double>(); 

        public static void BuildFile(AssemblyFile file, ControlFlowGraph graph)
        {
            Graph = graph;

            AddStrings(file, graph);
            AddFunctions(file, graph);
            AddReals(file);
        }

        private static void AddStrings(AssemblyFile file, ControlFlowGraph graph)
        {
            foreach (var str in graph.Strings)
            {
                file.DataSection.DataEntries.Add(new StringEntry(str.Key, str.Value));
            }
        }

        private static void AddReals(AssemblyFile file)
        {
            foreach (var real in Reals)
            {
                file.DataSection.DataEntries.Add(new RealEntry("real" + Math.Abs(real.GetHashCode()), real));
            }
        }

        private static void AddFunctions(AssemblyFile file, ControlFlowGraph graph)
        {
            foreach (var function in graph.Functions)
            {
                currentFunction = function.Key;
                AddFunction(file, function.Key, function.Value);
            }
        }

        private static void AddFunction(
            AssemblyFile file,
            string name,
            IEnumerable<BasicBlock> functionBlocks)
        {
            var parameterOffsets = new Dictionary<string, int>();
            currentOffset = 0;

            var procedure = new Procedure(name);

            for (int i = 0; i < Graph.FunctionParameters[name].Count; i++)
            {
                var param = Graph.FunctionParameters[name][i];
                var destination = GetVariableDestination(param, parameterOffsets);

                var register = GetArgumentRegister(i);
                var block = new Block(name + "params");

                block.Instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, register.ToString()));
                procedure.Blocks.Add(block);
            }

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
            else if (statement is ReturningCallStatement) instructions = CreateInstruction((ReturningCallStatement)statement, parameterOffsets);
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

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ParamStatement paramStatement,
            IDictionary<string, int> parameterOffsets)
        {
            var paramReg = GetArgumentRegister(currentParam++);

            return PlaceArgumentInRegister(paramReg, paramStatement.Argument, parameterOffsets);
        }

        private static Register GetArgumentRegister(int argumentNum)
        {
            switch (argumentNum)
            {
                case 0:
                    return Register.RCX;
                case 1:
                    return Register.RDX;
                case 2:
                    return Register.R8;
                case 3:
                    return Register.R9;
                default:
                    throw new ArgumentOutOfRangeException("currentParam");
            }
        }

        private static IEnumerable<Instruction> CreateInstruction(
            CallStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            currentParam = 0;

            string name = statement.Function.Name;

            yield return new SingleOpcodeInstruction(SingleArgOpcode.CALL, name);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ReturningCallStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            currentParam = 0;
            var name = statement.Function.Name;

            yield return new SingleOpcodeInstruction(SingleArgOpcode.CALL, name);

            var destination = GetVariableDestination(statement.Return, parameterOffsets);
            yield return new OpCodeInstruction(Opcode.MOV, destination, Register.RAX.ToString());
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ReturnStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(PlaceArgumentInRegister(Register.RAX, statement.Value, parameterOffsets));
            instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.JMP, currentFunction + "exit"));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            AssignStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();
            var destination = GetVariableDestination(statement.Return, parameterOffsets);

            instructions.AddRange(PlaceArgumentInRegister(Register.R10, statement.Argument, parameterOffsets));
            instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, Register.R10.ToString()));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            IEnumerable<Instruction> instructions;

            if (statement.Operator == BinaryOperator.Divide || 
                statement.Operator == BinaryOperator.Add || 
                statement.Operator == BinaryOperator.Equal || 
                statement.Operator == BinaryOperator.Multiply || 
                statement.Operator == BinaryOperator.Subtract ||
                statement.Operator == BinaryOperator.Mod || 
                statement.Operator == BinaryOperator.Exponensiation)
            {
                instructions = CreateMathInstruction(statement, parameterOffsets);
            }
            else if (statement.Operator == BinaryOperator.Equal ||
                statement.Operator == BinaryOperator.Greater ||
                statement.Operator == BinaryOperator.GreaterEqual ||
                statement.Operator == BinaryOperator.Less ||
                statement.Operator == BinaryOperator.LessEqual ||
                statement.Operator == BinaryOperator.NotEqual)
            {
                instructions = CreateComparisonInstruction(statement, parameterOffsets);
            }
            else
            {
                throw new ArgumentOutOfRangeException("statement", "Unsupported operator");
            }

            return instructions;
        }

        private static IEnumerable<Instruction> CreateMathInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {

            var argument1 = Register.R10;
            var argument2 = Register.R11;

            Opcode opcode;

            if (statement.Return.Type.PrimitiveType == PrimitiveType.Double)
            {
                argument1 = Register.XMM0;
                argument2 = Register.XMM1;

                switch (statement.Operator)
                {
                    case BinaryOperator.Add:
                        opcode = Opcode.ADDSD;
                        break;
                    case BinaryOperator.Subtract:
                        opcode = Opcode.SUBSD;
                        break;
                    case BinaryOperator.Multiply:
                        opcode = Opcode.MULSD;
                        break;
                    case BinaryOperator.Divide:
                        opcode = Opcode.DIVSD;
                        break;
                    case BinaryOperator.Exponensiation:
                        return CreateExponentiantionInstruction(statement, parameterOffsets);
                    default:
                        throw new ArgumentException("operation operation not supported");
                }
            }
            else
            {
                switch (statement.Operator)
                {
                    case BinaryOperator.Add:
                        opcode = Opcode.ADD;
                        break;
                    case BinaryOperator.Subtract:
                        opcode = Opcode.SUB;
                        break;
                    case BinaryOperator.Multiply:
                        opcode = Opcode.IMUL;
                        break;
                    case BinaryOperator.Divide:
                        return CreateDivisionInstruction(statement, parameterOffsets);
                    case BinaryOperator.Mod:
                        return CreateModuloInstruction(statement, parameterOffsets);
                    case BinaryOperator.Exponensiation:
                        return CreateExponentiantionInstruction(statement, parameterOffsets);
                    default:
                        throw new ArgumentException("operation operation not supported");
                } 
            }

            var destination = GetVariableDestination(statement.Return, parameterOffsets);
            var instructions = new List<Instruction>();

            instructions.AddRange(PlaceArgumentInRegister(argument1, statement.Left, parameterOffsets));
            instructions.AddRange(PlaceArgumentInRegister(argument2, statement.Right, parameterOffsets));
            instructions.Add(new OpCodeInstruction(opcode, argument1.ToString(), argument2.ToString()));

            if (statement.Return.Type.PrimitiveType == PrimitiveType.Double)
            {
                instructions.Add(new OpCodeInstruction(Opcode.MOVD, Register.R9.ToString(), argument1.ToString()));  
                instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, Register.R9.ToString()));  
            }
            else
            {
                instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, argument1.ToString()));    
            }
            
            return instructions;
        }

        private static IEnumerable<Instruction> CreateDivisionInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var destination = GetVariableDestination(statement.Return, parameterOffsets);

            // Clear RDX
            yield return new OpCodeInstruction(Opcode.XOR, Register.RDX.ToString(), Register.RDX.ToString());

            foreach (var instruction in PlaceArgumentInRegister(Register.RAX, statement.Left, parameterOffsets))
            {
                yield return instruction;
            }

            foreach (var instruction in PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets))
            {
                yield return instruction;
            }

            yield return new SingleOpcodeInstruction(SingleArgOpcode.IDIV, Register.R11.ToString());
            yield return new OpCodeInstruction(Opcode.MOV, destination, Register.RAX.ToString());
        }

        private static IEnumerable<Instruction> CreateModuloInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();
            var destination = GetVariableDestination(statement.Return, parameterOffsets);

            // Clear RDX
            instructions.Add(new OpCodeInstruction(Opcode.XOR, Register.RDX.ToString(), Register.RDX.ToString()));

            instructions.AddRange(PlaceArgumentInRegister(Register.RAX, statement.Left, parameterOffsets));
            instructions.AddRange(PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets));

            instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, Register.R11.ToString()));
            instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, Register.RDX.ToString()));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateExponentiantionInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();
            var destination = GetVariableDestination(statement.Return, parameterOffsets);

            if (Equals(statement.Return.Type, Type.DoubleType))
            {
                instructions.AddRange(PlaceArgumentInRegister(Register.XMM0, statement.Left, parameterOffsets));
                instructions.AddRange(PlaceArgumentInRegister(Register.XMM1, statement.Right, parameterOffsets));
                
                instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.CALL, "Power"));
                instructions.Add(new OpCodeInstruction(Opcode.MOVD, Register.R10.ToString(), Register.XMM0.ToString()));
                instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, Register.R10.ToString()));
            }
            else
            {
                instructions.AddRange(PlaceArgumentInRegister(Register.RCX, statement.Left, parameterOffsets));
                instructions.AddRange(PlaceArgumentInRegister(Register.RDX, statement.Right, parameterOffsets));
                instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.CALL, "Power"));
                instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, Register.RAX.ToString()));
            }

            return instructions;
        }

        private static IEnumerable<Instruction> CreateComparisonInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();
            var destination = GetVariableDestination(statement.Return, parameterOffsets);

            instructions.AddRange(PlaceArgumentInRegister(Register.R10, statement.Left, parameterOffsets));
            instructions.AddRange(PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets));

            Opcode opcode;
            switch (statement.Operator)
            {
                case BinaryOperator.Equal:
                    opcode = Opcode.CMOVE;
                    break;
                case BinaryOperator.NotEqual:
                    opcode = Opcode.CMOVNE;
                    break;
                case BinaryOperator.Less:
                    opcode = Opcode.CMOVL;
                    break;
                case BinaryOperator.LessEqual:
                    opcode = Opcode.CMOVLE;
                    break;
                case BinaryOperator.Greater:
                    opcode = Opcode.CMOVG;
                    break;
                case BinaryOperator.GreaterEqual:
                    opcode = Opcode.CMOVGE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("statement", "Unsupported operator");
            }

            instructions.Add(new OpCodeInstruction(Opcode.XOR, Register.RAX.ToString(), Register.RAX.ToString()));
            instructions.Add(new OpCodeInstruction(Opcode.CMP, Register.R10.ToString(), Register.R11.ToString()));

            instructions.AddRange(PlaceArgumentInRegister(Register.R8, new IntConstantArgument(1), parameterOffsets));
            instructions.Add(new OpCodeInstruction(opcode, Register.RAX.ToString(), Register.R8.ToString()));

            instructions.Add(new OpCodeInstruction(Opcode.MOV, destination, Register.RAX.ToString()));

            return instructions;
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
            var instructions = new List<Instruction>();

           instructions.AddRange(PlaceArgumentInRegister(Register.R10, statement.Left, parameterOffsets));
            instructions.AddRange(PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets));
            instructions.Add(new OpCodeInstruction(Opcode.CMP, Register.R10.ToString(), Register.R11.ToString()));

            SingleArgOpcode opcode;

            switch (statement.Operator)
            {
                case BinaryOperator.Less:
                    opcode = statement.Zero ? SingleArgOpcode.JG : SingleArgOpcode.JL;
                    break;
                case BinaryOperator.LessEqual:
                    opcode = statement.Zero ? SingleArgOpcode.JGE : SingleArgOpcode.JLE;
                    break;
                case BinaryOperator.Greater:
                    opcode = statement.Zero ? SingleArgOpcode.JL : SingleArgOpcode.JG;
                    break;
                case BinaryOperator.GreaterEqual:
                    opcode = statement.Zero ? SingleArgOpcode.JLE : SingleArgOpcode.JGE;
                    break;
                case BinaryOperator.Equal:
                    opcode = statement.Zero ? SingleArgOpcode.JNE : SingleArgOpcode.JE;
                    break;
                case BinaryOperator.NotEqual:
                    opcode = statement.Zero ? SingleArgOpcode.JE : SingleArgOpcode.JNE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            instructions.Add(new SingleOpcodeInstruction(opcode, "L" + statement.BranchTarget.Id));

            return instructions;
        }

        private static IEnumerable<Instruction> PlaceArgumentInRegister(
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
                argument2 = ((IntConstantArgument)argument).Value.ToString(CultureInfo.InvariantCulture);
            }
            else if (argument is BooleanConstantArgument)
            {
                argument2 = Convert.ToInt32(((BooleanConstantArgument)argument).Value).ToString(CultureInfo.InvariantCulture);
            }
            else if (argument is DoubleConstantArgument)
            {
                var value = ((DoubleConstantArgument)argument).Value;
                if (!Reals.Contains(value))
                {
                    Reals.Add(value);
                }

                argument2 = "real" + Math.Abs(value.GetHashCode());

                if (register != Register.XMM0 && register != Register.XMM1)
                {
                    return new[]
                               {
                                   new OpCodeInstruction(Opcode.MOVSD, Register.XMM2.ToString(), argument2),
                                   new OpCodeInstruction(Opcode.MOVD, register.ToString(), Register.XMM2.ToString())
                               };
                }

                return new[] { new OpCodeInstruction(Opcode.MOVSD, register.ToString(), argument2) };
            }
            else if (argument is VariableArgument)
            {
                argument2 = GetVariableDestination(((VariableArgument)argument).Variable, parameterOffsets);
                
                if (register == Register.XMM0 || register == Register.XMM1)
                {
                    return new[]
                               {
                                   new OpCodeInstruction(Opcode.MOV, Register.R8.ToString(), argument2),
                                   new OpCodeInstruction(Opcode.MOVD, register.ToString(), Register.R8.ToString())
                               };
                }
                
                return new[] { new OpCodeInstruction(code, argument1, argument2) };
            }
            else
            {
                throw new Exception();
            }

            return new[] { new OpCodeInstruction(code, argument1, argument2) };
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
                varOffset = currentOffset -= 8;
                parameterOffsets[variableSymbol.Name] = currentOffset;
            }

            return varOffset;
        }
    }
}
