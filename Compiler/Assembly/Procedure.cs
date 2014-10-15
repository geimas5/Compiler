namespace Compiler.Assembly
{
    using System.Collections.Generic;
    using System.IO;

    public class Procedure : AssemblyObject
    {
        public Procedure(string name)
        {
            this.Name = name;
            this.Blocks = new List<Block>();
        }

        public string Name { get; set; }

        public IList<Block> Blocks { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine(this.Name + " PROC");

            writer.WriteLine("push rbp");
            writer.WriteLine("mov rbp, rsp");
            writer.WriteLine("sub rsp, 0CCh"); // Allocate stackframe space.

            foreach (var block in this.Blocks)
            {
                block.Write(writer);
            }

            writer.WriteLine("{0}exit:", this.Name);

            if (this.Name == "main")
            {
                writer.WriteLine("call exit");
            }

            writer.WriteLine("mov rsp, rbp");
            writer.WriteLine("pop rbp");
            writer.WriteLine("ret");
            writer.WriteLine(this.Name + " ENDP");
        }
    }
}
