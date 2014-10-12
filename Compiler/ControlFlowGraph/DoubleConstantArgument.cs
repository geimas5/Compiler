namespace Compiler.ControlFlowGraph
{
    using System.Globalization;

    public class DoubleConstantArgument : Argument
    {
        public DoubleConstantArgument(double value)
        {
            this.Value = value;
        }

        public double Value { get; private set; }
        
        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
