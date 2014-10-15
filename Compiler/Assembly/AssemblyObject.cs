namespace Compiler.Assembly
{
    using System;
    using System.IO;

    public abstract class AssemblyObject
    {
        public abstract void Write(TextWriter writer);

        protected string GetDataType(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Byte:
                    return "byte";
                default:
                    throw new ArgumentOutOfRangeException("dataType");
            }
        }
    }
}
