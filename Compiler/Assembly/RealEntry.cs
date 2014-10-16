namespace Compiler.Assembly
{
    using System.Globalization;
    using System.IO;

    public class RealEntry : DataEntry
    {
        public RealEntry(string name, double value)
            : base(name)
        {
            this.Value = value;
        }

        public double Value { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine("{0} REAL8 {1}", this.Name, this.Value.ToString(CultureInfo.InvariantCulture));
        }
    }
}
