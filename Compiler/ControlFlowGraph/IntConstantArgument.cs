namespace Compiler.ControlFlowGraph
{
    using System.Globalization;

    public class IntConstantArgument : Argument
    {
        public IntConstantArgument(long value)
            : base(Type.IntType)
        {
            this.Value = value;
        }

        public long Value { get; private set; }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
