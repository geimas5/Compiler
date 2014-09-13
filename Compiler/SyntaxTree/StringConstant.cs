namespace Compiler.SyntaxTree
{
    public class StringConstant : ConstantNode
    {
        public StringConstant(Location location, string text)
            : base(location)
        {
            this.Text = text;
        }

        public string Text { get; set; }

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
            return base.ToString() + string.Format("(Text: {0})", this.Text);
        }
    }
}
