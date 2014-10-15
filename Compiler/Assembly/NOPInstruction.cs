namespace Compiler.Assembly
{
    using System.IO;

    public class NOPInstruction : Instruction
    {
        public override void Write(TextWriter writer)
        {
            writer.WriteLine("NOP");
        }
    }
}
