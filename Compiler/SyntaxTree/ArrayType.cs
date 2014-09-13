namespace Compiler.SyntaxTree
{
    public class ArrayType : TypeNode
    {
        public ArrayType(Location location, TypeNode baseType)
            : base(location)
        {
            this.BaseType = baseType;
        }

        public TypeNode BaseType { get; set; }

        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
