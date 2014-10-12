namespace Compiler.ControlFlowGraph
{
    using System.Globalization;

    public class BooleanConstantArgument : Argument
    {
        public BooleanConstantArgument(bool value)
        {
            this.Value = value;
        }

        public bool Value { get; set; }

        public override string ToString()
        {
            return this.Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
