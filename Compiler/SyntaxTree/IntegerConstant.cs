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

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return base.ToString() + string.Format("(Value: {0})", this.Value);
        }
    }
}
