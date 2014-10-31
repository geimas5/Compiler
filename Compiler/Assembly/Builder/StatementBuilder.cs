namespace Compiler.Assembly.Builder
{
    using System.Collections.Generic;

    using Compiler.Common;
    using Compiler.ControlFlowGraph;

    public abstract class StatementBuilder<T> where T : Statement
    {
        public T Statement { get; private set; }

        public Procedure Procedure { get; private set; }

        private IList<Instruction> instructions = new List<Instruction>();

        public IEnumerable<Instruction> Build(T statement, Procedure procedure)
        {
            instructions = new List<Instruction>();

            this.Statement = statement;
            this.Procedure = procedure;
            this.Build();

            return this.instructions;
        }

        protected abstract void Build();

        protected void WriteInstruction(Instruction instruction)
        {
            instructions.Add(instruction);
        }

        protected void WriteBinaryInstruction(Opcode opcode, Operand argument1, Operand argument2)
        {
            this.WriteInstruction(new BinaryOpCodeInstruction(opcode, argument1, argument2));
        }

        protected void PlaceArgumentInRegister(Argument argument, Register registe)
        {
            instructions.AddRange(BuilderHelper.PlaceArgumentInRegister(registe, argument, Procedure));
        }

        protected void WriteRegisterToDestination(Destination destination, Register register)
        {
            instructions.AddRange(BuilderHelper.WriteRegisterToDestination(destination, register, Procedure));
        }
    }
}
