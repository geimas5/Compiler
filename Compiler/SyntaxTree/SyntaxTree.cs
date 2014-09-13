namespace Compiler.SyntaxTree
{
    public class SyntaxTree
    {
        public ProgramNode RootNode { get; set; }

        public override string ToString()
        {
            return ((object)this.RootNode ?? string.Empty).ToString();
        }
    }
}
