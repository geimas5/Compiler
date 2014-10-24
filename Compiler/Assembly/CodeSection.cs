namespace Compiler.Assembly
{
    using System.Collections.Generic;
    using System.IO;

    public class CodeSection : Section
    {
        public CodeSection()
            : base("Code")
        {
            this.Procedures = new List<Procedure>();
        }

        public IList<Procedure> Procedures { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine(".code");

            foreach (var procedure in this.Procedures)
            {
                procedure.Write(writer);
            }
        }
    }
}
