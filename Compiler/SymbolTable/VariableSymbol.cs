namespace Compiler.SymbolTable
{
    using System;

    using Type = Compiler.Type;

    public class VariableSymbol : ITypedSymbol
    {
        protected bool Equals(VariableSymbol other)
        {
            return string.Equals(this.Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (this.Name != null ? this.Name.GetHashCode() : 0);
        }

        public VariableSymbol(string name, Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }
            
            this.Type = type;
            this.Name = name;
        }

        public string Name { get; private set; }

        public Type Type { get; private set; }


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

            return Equals((VariableSymbol)obj);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
