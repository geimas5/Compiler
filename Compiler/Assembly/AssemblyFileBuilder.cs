namespace Compiler.Assembly
{
    using System;
    using System.Collections.Generic;

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

        private static readonly List<double> Reals = new List<double>(); 

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

            var paramsBlock = new Block(name + "params");
            for (int i = 0; i < Graph.FunctionParameters[name].Count; i++)
            {
                var param = Graph.FunctionParameters[name][i];
                var destination = GetVariableDestination(param, parameterOffsets);

                var register = GetArgumentRegister(i);

                paramsBlock.Instructions.Add(new BinaryOpCodeInstruction(Opcode.MOV, destination, new RegisterOperand(register)));    
            }

            procedure.Blocks.Add(paramsBlock);

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

            procedure.NumberOfLocalVariables = Math.Abs(currentOffset) / 8;
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
            else if (statement is ConvertToDoubleStatement) instructions = CreateInstruction((ConvertToDoubleStatement)statement, parameterOffsets);
            else if (statement is AllocStatement) instructions = CreateInstruction((AllocStatement)statement, parameterOffsets);
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

            yield return new CallInstruction(name);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ReturningCallStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            currentParam = 0;
            var name = statement.Function.Name;

            var instructions = new List<Instruction>();

            instructions.Add(new CallInstruction(name));

            instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.RAX, parameterOffsets));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ReturnStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(PlaceArgumentInRegister(Register.RAX, statement.Value, parameterOffsets));
            instructions.Add(new JumpInstruction(JumpOpCodes.JMP, currentFunction + "exit"));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            AssignStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();
            
            instructions.AddRange(PlaceArgumentInRegister(Register.R10, statement.Argument, parameterOffsets));
            instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.R10, parameterOffsets));

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

            var instructions = new List<Instruction>();

            instructions.AddRange(PlaceArgumentInRegister(argument1, statement.Left, parameterOffsets));
            instructions.AddRange(PlaceArgumentInRegister(argument2, statement.Right, parameterOffsets));
            instructions.Add(new BinaryOpCodeInstruction(opcode, new RegisterOperand(argument1), new RegisterOperand(argument2)));

            if (statement.Return.Type.PrimitiveType == PrimitiveType.Double)
            {
                instructions.Add(new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(Register.R9), new RegisterOperand(argument1)));  
                instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.R9, parameterOffsets));
            }
            else
            {
                instructions.AddRange(WriteRegisterToDestination(statement.Return, argument1, parameterOffsets));
            }
            
            return instructions;
        }

        private static IEnumerable<Instruction> CreateDivisionInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();
                
            // Clear RDX
            instructions.Add(new BinaryOpCodeInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX)));

            foreach (var instruction in PlaceArgumentInRegister(Register.RAX, statement.Left, parameterOffsets))
            {
                instructions.Add(instruction);
            }

            foreach (var instruction in PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets))
            {
                instructions.Add(instruction);
            }

            instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, new RegisterOperand(Register.R11)));
            instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.RAX, parameterOffsets));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateModuloInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            // Clear RDX
            instructions.Add(new BinaryOpCodeInstruction(Opcode.XOR, new RegisterOperand(Register.RDX), new RegisterOperand(Register.RDX)));

            instructions.AddRange(PlaceArgumentInRegister(Register.RAX, statement.Left, parameterOffsets));
            instructions.AddRange(PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets));

            instructions.Add(new SingleOpcodeInstruction(SingleArgOpcode.IDIV, new RegisterOperand(Register.R11)));

            instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.RDX, parameterOffsets));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateExponentiantionInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            if (Equals(statement.Return.Type, Type.DoubleType))
            {
                instructions.AddRange(PlaceArgumentInRegister(Register.XMM0, statement.Left, parameterOffsets));
                instructions.AddRange(PlaceArgumentInRegister(Register.XMM1, statement.Right, parameterOffsets));
                
                instructions.Add(new CallInstruction("Power"));
                instructions.Add(new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), new RegisterOperand(Register.XMM0)));
                instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.R10, parameterOffsets));
            }
            else
            {
                instructions.AddRange(PlaceArgumentInRegister(Register.RCX, statement.Left, parameterOffsets));
                instructions.AddRange(PlaceArgumentInRegister(Register.RDX, statement.Right, parameterOffsets));
                instructions.Add(new CallInstruction("Power"));
                instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.RAX, parameterOffsets));
            }

            return instructions;
        }

        private static IEnumerable<Instruction> CreateComparisonInstruction(
            BinaryOperatorStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

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

            instructions.Add(new BinaryOpCodeInstruction(Opcode.XOR, new RegisterOperand(Register.RAX), new RegisterOperand(Register.RAX)));
            instructions.Add(new BinaryOpCodeInstruction(Opcode.CMP, new RegisterOperand(Register.R10), new RegisterOperand(Register.R11)));

            instructions.AddRange(PlaceArgumentInRegister(Register.R8, new IntConstantArgument(1), parameterOffsets));
            instructions.Add(new BinaryOpCodeInstruction(opcode, new RegisterOperand(Register.RAX), new RegisterOperand(Register.R8)));

            instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.RAX, parameterOffsets));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            JumpStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            yield return new JumpInstruction(JumpOpCodes.JMP, "L" + statement.Target.Id);
        }

        private static IEnumerable<Instruction> CreateInstruction(
            BranchStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(PlaceArgumentInRegister(Register.R10, statement.Left, parameterOffsets));
            instructions.AddRange(PlaceArgumentInRegister(Register.R11, statement.Right, parameterOffsets));
            instructions.Add(new BinaryOpCodeInstruction(Opcode.CMP, new RegisterOperand(Register.R10), new RegisterOperand(Register.R11)));

            JumpOpCodes opcode;

            switch (statement.Operator)
            {
                case BinaryOperator.Less:
                    opcode = statement.Zero ? JumpOpCodes.JG : JumpOpCodes.JL;
                    break;
                case BinaryOperator.LessEqual:
                    opcode = statement.Zero ? JumpOpCodes.JGE : JumpOpCodes.JLE;
                    break;
                case BinaryOperator.Greater:
                    opcode = statement.Zero ? JumpOpCodes.JL : JumpOpCodes.JG;
                    break;
                case BinaryOperator.GreaterEqual:
                    opcode = statement.Zero ? JumpOpCodes.JLE : JumpOpCodes.JGE;
                    break;
                case BinaryOperator.Equal:
                    opcode = statement.Zero ? JumpOpCodes.JNE : JumpOpCodes.JE;
                    break;
                case BinaryOperator.NotEqual:
                    opcode = statement.Zero ? JumpOpCodes.JE : JumpOpCodes.JNE;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            instructions.Add(new JumpInstruction(opcode, "L" + statement.BranchTarget.Id));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ConvertToDoubleStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(PlaceArgumentInRegister(Register.R11, statement.Argument, parameterOffsets));
            instructions.Add(new BinaryOpCodeInstruction(Opcode.CVTSI2SD, new RegisterOperand(Register.XMM0), new RegisterOperand(Register.R11)));

            instructions.Add(new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), new RegisterOperand(Register.XMM0)));

            instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.R10, parameterOffsets));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            AllocStatement statement,
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(PlaceArgumentInRegister(GetArgumentRegister(0), statement.Size, parameterOffsets));
            instructions.Add(new CallInstruction("Alloc"));
            instructions.AddRange(WriteRegisterToDestination(statement.Return, Register.RAX, parameterOffsets));

            return instructions;
        }

        private static IEnumerable<Instruction> PlaceArgumentInRegister(
            Register register,
            Argument argument,
            IDictionary<string, int> parameterOffsets)
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
                if (!Reals.Contains(value))
                {
                    Reals.Add(value);
                }

                argument2 = new DataOperand("real" + Math.Abs(value.GetHashCode()));

                if (register != Register.XMM0 && register != Register.XMM1)
                {
                    return new[]
                               {
                                   new BinaryOpCodeInstruction(Opcode.MOVSD, new RegisterOperand(Register.XMM2), argument2),
                                   new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(register), new RegisterOperand(Register.XMM2))
                               };
                }

                return new[] { new BinaryOpCodeInstruction(Opcode.MOVSD, new RegisterOperand(register), argument2) };
            }
            else if (argument is VariableArgument)
            {
                argument2 = GetVariableDestination(((VariableArgument)argument).Variable, parameterOffsets);
                
                if (register == Register.XMM0 || register == Register.XMM1)
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

                instructions.AddRange(
                    PlaceArgumentInRegister(
                        Register.R12,
                        new VariableArgument(pointerArgument.Variable),
                        parameterOffsets));

                instructions.Add(
                    new BinaryOpCodeInstruction(
                        Opcode.MOV,
                        new RegisterOperand(register),
                        new MemoryOperand(Register.R12)));

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
            IDictionary<string, int> parameterOffsets)
        {
            var instructions = new List<Instruction>();

            var variableDestination = destination as VariableDestination;
            if (variableDestination != null)
            {
                var destinationOperand = GetVariableDestination(
                    variableDestination.Variable,
                    parameterOffsets);

                instructions.Add(new BinaryOpCodeInstruction(Opcode.MOV, destinationOperand, new RegisterOperand(register)));    
            }
            else if (destination is PointerDestination)
            {
                var pointerDestination = (PointerDestination)destination;

                instructions.AddRange(PlaceArgumentInRegister(
                    Register.R12,
                    new VariableArgument(pointerDestination.Destination),
                    parameterOffsets));

                instructions.Add(
                    new BinaryOpCodeInstruction(
                        Opcode.MOV,
                        new MemoryOperand(Register.R12),
                        new RegisterOperand(register)));
            }

            return instructions;
        }

        private static MemoryOperand GetVariableDestination(
            VariableSymbol destination,
            IDictionary<string, int> parameterOffsets)
        {
            var variableOffset = GetVariableOffset(destination, parameterOffsets);

            return new MemoryOperand(Register.RBP, variableOffset);
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
