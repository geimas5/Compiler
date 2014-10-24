namespace Compiler.Assembly
{
    public class DataOperand : Operand
    {
        public DataOperand(string name)
        {
            this.Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
