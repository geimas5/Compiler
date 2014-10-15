namespace Compiler.Assembly
{
    using System.IO;

    public class StringEntry : DataEntry
    {
        public StringEntry(string name, string value)
            : base(name)
        {
            this.Value = value;
        }

        public string Value { get; private set; }

        public override void Write(TextWriter writer)
        {
            if (string.IsNullOrEmpty(this.Value))
            {
                writer.WriteLine("{0} db 0", this.Name);   
            }
            else
            {
                string val = Value.Replace("\"", "\"\"");

                writer.WriteLine("{0} db \"{1}\",0", this.Name, val);
            }
        }
    }
}
