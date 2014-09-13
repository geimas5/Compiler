namespace Compiler.SyntaxTree
{
    public class IntegerConstant : ConstantNode
    {
        public IntegerConstant(Location location, int value)
            : base(location)
        {
            this.Value = value;
        }

        public int Value { get; set; }
    }
}
