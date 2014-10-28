namespace Compiler.Assembly.Builder
{
    using System;
    using System.Collections.Generic;

    using Compiler.Common;
    using Compiler.ControlFlowGraph;
    using Compiler.SyntaxTree;

    using ReturnStatement = Compiler.ControlFlowGraph.ReturnStatement;

    public static class AssemblyFileBuilder
    {
        private static int currentParam = 0;

        private static string currentFunction = string.Empty;

        private static Procedure currentProcedure;

        private static ControlFlowGraph graph;

        public static void BuildFile(AssemblyFile file, ControlFlowGraph graph)
        {
            AssemblyFileBuilder.graph = graph;

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
                currentFunction = function.Key;
                AddFunction(file, function.Key, function.Value);
            }
        }

        private static void AddFunction(
            AssemblyFile file,
            string name,
            IEnumerable<BasicBlock> functionBlocks)
        {
            currentProcedure = new Procedure(name, file);

            var paramsBlock = new Block(name + "params");
            for (int i = 0; i < graph.FunctionParameters[name].Count; i++)
            {
                var param = graph.FunctionParameters[name][i];
                var register = GetArgumentRegister(i);

                paramsBlock.Instructions.Add(
                    new BinaryOpCodeInstruction(
                        Opcode.MOV,
                        currentProcedure.GetVarialeLocation(param),
                        new RegisterOperand(register)));
            }

            currentProcedure.Blocks.Add(paramsBlock);

            foreach (var block in functionBlocks)
            {
                var assemblyBlock = new Block("L" + block.Enter.Id);
                currentProcedure.Blocks.Add(assemblyBlock);

                foreach (var statement in block)
                {
                    assemblyBlock.Instructions.AddRange(CreateInstruction(statement));
                }
            }

            file.CodeSection.Procedures.Add(currentProcedure);
        }

        private static IEnumerable<Instruction> CreateInstruction(Statement statement)
        {
            IEnumerable<Instruction> instructions;

            if (statement is ParamStatement) instructions = CreateInstruction((ParamStatement)statement);
            else if (statement is ReturningCallStatement) instructions = CreateInstruction((ReturningCallStatement)statement);
            else if (statement is CallStatement) instructions = CreateInstruction((CallStatement)statement);
            else if (statement is AssignStatement) instructions = CreateInstruction((AssignStatement)statement);
            else if (statement is ReturnStatement) instructions = CreateInstruction((ReturnStatement)statement);
            else if (statement is BinaryOperatorStatement) return BinaryOperatorBuilder.BuildOperator((BinaryOperatorStatement)statement, currentProcedure);
            else if (statement is JumpStatement) instructions = CreateInstruction((JumpStatement)statement);
            else if (statement is BranchStatement) instructions = CreateInstruction((BranchStatement)statement);
            else if (statement is ConvertToDoubleStatement) instructions = CreateInstruction((ConvertToDoubleStatement)statement);
            else if (statement is AllocStatement) instructions = CreateInstruction((AllocStatement)statement);
            else if (statement is NopStatement) instructions = new[] { new NOPInstruction() };
            else
            {
                throw new ArgumentException();
            }

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            ParamStatement paramStatement)
        {
            var paramReg = GetArgumentRegister(currentParam++);

            return BuilderHelper.PlaceArgumentInRegister(paramReg, paramStatement.Argument, currentProcedure);
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

        private static IEnumerable<Instruction> CreateInstruction(CallStatement statement)
        {
            currentParam = 0;

            string name = statement.Function.Name;

            yield return new BinaryOpCodeInstruction(Opcode.SUB, new RegisterOperand(Register.RSP), new ConstantOperand(40));
            yield return new CallInstruction(name);
            yield return new BinaryOpCodeInstruction(Opcode.ADD, new RegisterOperand(Register.RSP), new ConstantOperand(40));
        }

        private static IEnumerable<Instruction> CreateInstruction(ReturningCallStatement statement)
        {
            currentParam = 0;
            var name = statement.Function.Name;

            var instructions = new List<Instruction>();

            instructions.Add(new CallInstruction(name));

            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.RAX, currentProcedure));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(ReturnStatement statement)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.RAX, statement.Value, currentProcedure));
            instructions.Add(new JumpInstruction(JumpOpCodes.JMP, currentFunction + "exit"));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(AssignStatement statement)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R10, statement.Argument, currentProcedure));
            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.R10, currentProcedure));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            JumpStatement statement)
        {
            yield return new JumpInstruction(JumpOpCodes.JMP, "L" + statement.Target.Id);
        }

        private static IEnumerable<Instruction> CreateInstruction(BranchStatement statement)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R10, statement.Left, currentProcedure));
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R11, statement.Right, currentProcedure));
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

        private static IEnumerable<Instruction> CreateInstruction(ConvertToDoubleStatement statement)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R11, statement.Argument, currentProcedure));
            instructions.Add(new BinaryOpCodeInstruction(Opcode.CVTSI2SD, new RegisterOperand(Register.XMM0), new RegisterOperand(Register.R11)));

            instructions.Add(new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), new RegisterOperand(Register.XMM0)));

            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.R10, currentProcedure));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(
            AllocStatement statement)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(GetArgumentRegister(0), statement.Size, currentProcedure));
            instructions.Add(new CallInstruction("Alloc"));
            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.RAX, currentProcedure));

            return instructions;
        }
    }
}
