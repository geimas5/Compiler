namespace Compiler.ControlFlowGraph
{
    using System;

    using Compiler.Assembly;

    public class BinaryOpCodeInstruction : Instruction
    {
        public BinaryOpCodeInstruction(Opcode opcode, Operand argument1, Operand argument2)
        {
            if (argument1 is MemoryOperand && argument2 is MemoryOperand)
            {
                throw new ArgumentException("At most one of the operands may be memory");
            }

            if (argument1 is ConstantOperand)
            {
                throw new ArgumentException("The first argument may not be a constant", "argument1");
            }
            

            this.Opcode = opcode;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
        }

        public Opcode Opcode { get; set; }

        public Operand Argument1 { get; set; }
        public Operand Argument2 { get; set; }

        public override string InstructionText
        {
            get
            {
                return string.Format("{0} {1}, {2}", Opcode, this.Argument1, this.Argument2);
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }
}
