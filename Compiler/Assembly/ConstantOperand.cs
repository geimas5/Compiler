namespace Compiler.Assembly
{
    using System.Globalization;

    public class ConstantOperand : Operand
    {
        public ConstantOperand(long value)
        {
            this.Value = value;
        }

        public long Value { get; private set; }

        public override string ToString()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
