namespace Compiler.SyntaxTree
{
    public class PrimitiveType : TypeNode
    {
        public PrimitiveType(Location location, Types types)
            : base(location)
        {
            this.Type = types;
        }

        public Types Type { get; set; }

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
            return base.ToString() + string.Format("(Type: {0})", this.Type);
        }
    }
}
