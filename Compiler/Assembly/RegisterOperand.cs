namespace Compiler.Assembly
{
    public class RegisterOperand : Operand
    {
        public RegisterOperand(Register register)
        {
            this.Register = register;
        }

        public Register Register { get; private set; }

        public override string ToString()
        {
            return this.Register.ToString();
        }
    }
}
