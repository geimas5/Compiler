namespace Compiler.Assembly.Builder
{
    using System;
    using System.Collections.Generic;

    using Compiler.Common;
    using Compiler.ControlFlowGraph;

    using ReturnStatement = Compiler.ControlFlowGraph.ReturnStatement;
    using Type = Compiler.Type;

    public static class AssemblyFileBuilder
    {
        private static string currentFunction = string.Empty;

        private static Procedure currentProcedure;

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

            if (statement is CallStatement) return new CallStatementBuilder().Build((CallStatement)statement, currentProcedure);
            if (statement is AssignStatement) return new AssignStatementBuilder().Build((AssignStatement)statement, currentProcedure);
            if (statement is ReturnStatement) instructions = CreateInstruction((ReturnStatement)statement);
            else if (statement is BinaryOperatorStatement) return new BinaryOperatorBuilder().Build((BinaryOperatorStatement)statement, currentProcedure);
            else if (statement is UnaryOperatorStatement) return new UnaryOperatorBuilder().Build((UnaryOperatorStatement)statement, currentProcedure);
            else if (statement is JumpStatement) instructions = CreateInstruction((JumpStatement)statement);
            else if (statement is BranchStatement) return new BranchStatementBuilder().Build((BranchStatement)statement, currentProcedure);
            else if (statement is ConvertToDoubleStatement) instructions = CreateInstruction((ConvertToDoubleStatement)statement);
            else if (statement is NopStatement) instructions = new[] { new NOPInstruction() };
            else
            {
                throw new ArgumentException();
            }

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(ReturnStatement statement)
        {
            var instructions = new List<Instruction>();

            if (statement.Value != null)
                instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.RAX, statement.Value, currentProcedure));

            instructions.Add(new JumpInstruction(JumpOpCodes.JMP, currentFunction + "exit"));

            return instructions;
        }

        private static IEnumerable<Instruction> CreateInstruction(JumpStatement statement)
        {
            yield return new JumpInstruction(JumpOpCodes.JMP, "L" + statement.Target.Id);
        }

        private static IEnumerable<Instruction> CreateInstruction(ConvertToDoubleStatement statement)
        {
            var instructions = new List<Instruction>();

            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(Register.R11, statement.Argument, currentProcedure));
            instructions.Add(new BinaryOpCodeInstruction(Opcode.CVTSI2SD, new RegisterOperand(Register.XMM14), new RegisterOperand(Register.R11)));

            instructions.Add(new BinaryOpCodeInstruction(Opcode.MOVD, new RegisterOperand(Register.R10), new RegisterOperand(Register.XMM14)));

            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(statement.Return, Register.R10, currentProcedure));

            return instructions;
        }
    }
}
