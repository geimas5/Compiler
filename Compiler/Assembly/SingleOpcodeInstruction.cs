namespace Compiler.Assembly
{
    using System.IO;

    public class SingleOpcodeInstruction : Instruction
    {
        public SingleOpcodeInstruction(SingleArgOpcode opcode, Operand argument)
        {
            this.Opcode = opcode;
            this.Argument = argument;
        }

        public SingleArgOpcode Opcode { get; set; }

        public Operand Argument { get; set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("{0} {1}", this.Opcode, this.Argument);
        }
    }
}
