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
    }
}
