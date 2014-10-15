namespace Compiler.ControlFlowGraph
{
    using System;

    using Compiler.Assembly;

    public class OpCodeInstruction : Instruction
    {
        public OpCodeInstruction(Opcode opcode, string argument1, string argument2)
        {
            this.Opcode = opcode;
            this.Argument1 = argument1;
            this.Argument2 = argument2;
        }

        public Opcode Opcode { get; set; }

        public string Argument1 { get; set; }
        public string Argument2 { get; set; }

        public override string InstructionText
        {
            get
            {
                return string.Format("{0} {1}, {2}", Opcode, Argument1, Argument2);
            }
            set
            {
                throw new ArgumentException();
            }
        }
    }
}
