namespace Compiler.DataFlowAnalysis
{
    using System.Collections;
    using System.Collections.Generic;

    using Antlr4.Runtime.Sharpen;

    using Compiler.SymbolTable;

    public class VariableBitset : IEnumerable<VariableSymbol>
    {
        private readonly BitSet internalBitset = new BitSet();

        private readonly VariableRegister register;

        public VariableBitset(VariableRegister register)
        {
            this.register = register;
        }

        private VariableBitset(VariableRegister register, BitSet bitset)
            : this(register)
        {
            this.internalBitset = bitset;
        }

        protected bool Equals(VariableBitset other)
        {
            return Equals(this.internalBitset, other.internalBitset);
        }

        public override int GetHashCode()
        {
            return (this.internalBitset != null ? this.internalBitset.GetHashCode() : 0);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public VariableBitset And(VariableBitset variableBitset)
        {
            this.internalBitset.And(variableBitset.internalBitset);

            return this;
        }

        public VariableBitset Or(VariableBitset variableBitset)
        {
            this.internalBitset.Or(variableBitset.internalBitset);

            return this;
        }

        public void Clear(VariableSymbol symbol)
        {
            this.internalBitset.Clear(this.register.GetVariableIndex(symbol));
        }

        public void Set(VariableSymbol symbol)
        {
            this.internalBitset.Set(this.register.GetVariableIndex(symbol));
        }

        public bool Get(VariableSymbol symbol)
        {
            return this.internalBitset.Get(this.register.GetVariableIndex(symbol));
        }

        public VariableBitset Clone()
        {
            return new VariableBitset(register, internalBitset.Clone());
        }

        public VariableBitset Invert()
        {
            for (int i = 0; i <= register.Count; i++)
            {
                if (internalBitset.Get(i))
                    internalBitset.Clear(i);
                else
                    internalBitset.Set(i);   
            }

            return this;
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

            return Equals((VariableBitset)obj);
        }

        public IEnumerator<VariableSymbol> GetEnumerator()
        {
            for (int i = this.internalBitset.NextSetBit(0); i != -1; i = this.internalBitset.NextSetBit(i + 1))
            {
                yield return this.register.GetVariable(i);
            }
        }

        public override string ToString()
        {
            string str = "{";

            for (int i = this.internalBitset.NextSetBit(0); i != -1; i = this.internalBitset.NextSetBit(i + 1))
            {
                str += this.register.GetVariable(i).Name + ",";
            }

            return str + "}";
        }
    }
}
