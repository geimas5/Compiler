namespace Compiler.ControlFlowGraph
{
    using System.Globalization;

    public class IntConstantArgument : Argument
    {
        public IntConstantArgument(int value)
        {
            this.Value = value;
        }

        public int Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
