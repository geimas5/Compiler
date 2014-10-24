namespace Compiler.Assembly
{
    using System.IO;

    public class CallInstruction : Instruction
    {
        public CallInstruction(string procedure)
        {
            this.Procedure = procedure;
        }

        public string Procedure { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("CALL {0}", this.Procedure);
        }
    }
}
