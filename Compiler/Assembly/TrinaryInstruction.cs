namespace Compiler.Assembly
{
    using System;

    public class TrinaryInstruction : Instruction
    {
        public TrinaryInstruction(TrinaryOpcode opcode, Operand argument1, Operand argument2, Operand argument3)
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
            this.Argument3 = argument3;
        }

        public TrinaryOpcode Opcode { get; set; }

        public Operand Argument1 { get; set; }
        public Operand Argument2 { get; set; }
        public Operand Argument3 { get; set; }

        public override string InstructionText

        {
            get
            {
                return string.Format("{0} {1}, {2}, {3}", Opcode, this.Argument1, this.Argument2, this.Argument3);
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }
}
