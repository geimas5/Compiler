namespace Compiler.Assembly
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class DataSection : Section
    {
        private static readonly Dictionary<double, string> Reals = new Dictionary<double, string>();

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

        public string AddOrGetRealId(double value)
        {
            if (!Reals.ContainsKey(value))
            {
                var key = "real" + Math.Abs(value.GetHashCode());
                this.DataEntries.Add(new RealEntry(key, value));
                Reals[value] = key;
            }

            return Reals[value];
        }
    }
}
