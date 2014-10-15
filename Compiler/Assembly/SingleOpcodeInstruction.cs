namespace Compiler.Assembly
{
    using System.IO;

    public class SingleOpcodeInstruction : Instruction
    {
        public SingleOpcodeInstruction(SingleArgOpcode opcode, string arg)
        {
            this.Opcode = opcode;
            this.Arg = arg;
        }

        public SingleArgOpcode Opcode { get; set; }

        public string Arg { get; set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("{0} {1}", this.Opcode, this.Arg);
        }
    }
}
