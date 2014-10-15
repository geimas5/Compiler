namespace Compiler.Assembly
{
    using System.IO;

    public class JumpInstruction : Instruction
    {
        public JumpInstruction(string target)
        {
            this.Target = target;
        }

        public string Target { get; set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("JMP {0}", this.Target);
        }
    }
}
