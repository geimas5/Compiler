namespace Compiler.Assembly
{
    using System;

    public class MemoryOperand : Operand
    {
        public MemoryOperand(Register register, int offset = 0)
        {
            this.Register = register;
            this.Offset = offset;
        }

        public Register Register { get; private set; }

        public int Offset { get; private set; }

        public override string ToString()
        {
            if (Offset < 0)
            {
                return string.Format("[{0} - {1}]", this.Register, Math.Abs(Offset));
            }

            if (Offset > 0)
            {
                return string.Format("[{0} + {1}]", this.Register, Math.Abs(Offset));
            }

            return string.Format("[{0}]", this.Register);
        }
    }
}
