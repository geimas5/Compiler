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
    }
}
