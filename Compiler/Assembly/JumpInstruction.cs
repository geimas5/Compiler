namespace Compiler.Assembly
{
    using System.IO;

    public class JumpInstruction : Instruction
    {
        public JumpInstruction(JumpOpCodes opCode, string target)
        {
            this.Target = target;
            this.OpCode = opCode;
        }

        public string Target { get; private set; }

        public JumpOpCodes OpCode { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("{0} {1}", OpCode, this.Target);
        }
    }
}
