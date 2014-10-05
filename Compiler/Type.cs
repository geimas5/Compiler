namespace Compiler
{
    using Compiler.SyntaxTree;

    public class Type
    {

        static Type noTypeInstance;

        public Type(PrimitiveType primitiveType)
        {
            this.Dimensions = 0;
            this.PrimitiveType = primitiveType;
        }

        public Type(PrimitiveType primitiveType, int dimensions)
            : this(primitiveType)
        {
            this.Dimensions = dimensions;
        } 

        public int Dimensions { get; private set; }

        public PrimitiveType PrimitiveType { get; private set; }

        public static Type NoType
        {
            get
            {
                return noTypeInstance ?? (noTypeInstance = new Type(PrimitiveType.NoType));
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Type)obj);
        }

        protected bool Equals(Type other)
        {
            return this.Dimensions == other.Dimensions && this.PrimitiveType == other.PrimitiveType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Dimensions * 397) ^ (int)this.PrimitiveType;
            }
        }
    }
}
