namespace Compiler.Assembly
{
    using System.IO;

    public class Instruction : AssemblyObject
    {
        public Instruction(string instructionText)
        {
            this.InstructionText = instructionText;
        }

        public Instruction()
        {
        }

        public virtual string InstructionText { get; set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine(this.InstructionText);
        }
    }
}
