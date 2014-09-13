
namespace Compiler.SyntaxTree
{
    using System;

    public class VariableNode : Node
    {
        public VariableNode(Location location, TypeNode type, VariableIdNode name)
            : base(location)
        {
            this.Type = type;
            this.Name = name;
        }

        public TypeNode Type { get; set; }

        public VariableIdNode Name { get; set; }

        public override string ToString()
        {
            string text = "(VariableNode " + this.Location + Environment.NewLine;

            text += ((object)this.Type ?? string.Empty) + Environment.NewLine;
            text += ((object)this.Name ?? string.Empty).ToString();

            return text + ")";
        }
    }
}
