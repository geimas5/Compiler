namespace Compiler.Assembly
{
    using System;

    using Type = Compiler.Type;

    public static class CallingConvention
    {
        public static  Register GetArgumentRegister(Type type, int argumentNum)
        {
            if (Equals(type, Type.DoubleType))
            {
                return GetDoubleArgumentRegister(argumentNum);
            }

            return GetGeneralArgumentRegister(argumentNum);
        }

        private static Register GetDoubleArgumentRegister(int argumentNum)
        {
            switch (argumentNum)
            {
                case 0:
                    return Register.XMM0;
                case 1:
                    return Register.XMM1;
                case 2:
                    return Register.XMM2;
                case 3:
                    return Register.XMM3;
                default:
                    throw new ArgumentOutOfRangeException("argumentNum");
            }
        }

        private static Register GetGeneralArgumentRegister(int argumentNum)
        {
            switch (argumentNum)
            {
                case 0:
                    return Register.RCX;
                case 1:
                    return Register.RDX;
                case 2:
                    return Register.R8;
                case 3:
                    return Register.R9;
                default:
                    throw new ArgumentOutOfRangeException("currentParam");
            }
        }

        public static int NumberOfRegisterParams
        {
            get
            {
                return 4;
            }
        }
    }
}
