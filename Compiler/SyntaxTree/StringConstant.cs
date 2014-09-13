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
    }
}
