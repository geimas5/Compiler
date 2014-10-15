namespace Compiler.Assembly
{
    using System.Collections.Generic;
    using System.IO;

    public class DataSection : Section
    {
        public DataSection()
            : base("data")
        {
            this.DataEntries = new List<DataEntry>();
        }

        public IList<DataEntry> DataEntries { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine(".data");

            foreach (var dataEntry in this.DataEntries)
            {
                dataEntry.Write(writer);
            }
        }
    }
}
