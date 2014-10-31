namespace Compiler.Assembly
{
    public static class RegisterUtility
    {
        public static bool IsXMM(Register register)
        {
            return register == Register.XMM0 || register == Register.XMM1 || register == Register.XMM2
                   || register == Register.XMM3 || register == Register.XMM4 || register == Register.XMM5
                   || register == Register.XMM6 || register == Register.XMM7 || register == Register.XMM8
                   || register == Register.XMM9 || register == Register.XMM10 || register == Register.XMM11
                   || register == Register.XMM12 || register == Register.XMM13 || register == Register.XMM14
                   || register == Register.XMM15;
        }
    }
}
