namespace Compiler.Assembly
{
    using System.Collections.Generic;
    using System.IO;

    public class Block : AssemblyObject
    {
        public Block(string label)
        {
            this.Label = label;
            this.Instructions = new List<Instruction>();
        }

        public string Label { get; set; }

        public IList<Instruction> Instructions { get; private set; }

        public override void Write(TextWriter writer)
        {
            writer.WriteLine(Label + ":");

            foreach (var instruction in Instructions)
            {
                instruction.Write(writer);
            }
        }
    }
}
