namespace Compiler.Assembly
{
    public static class RegisterUtility
    {
        public static bool IsXMM(Register register)
        {
            return register == Register.XMM0 || register == Register.XMM1 || register == Register.XMM2;
        }
    }
}
