namespace Compiler.Parser
{
    using Compiler.SyntaxTree;

    public class ParsingResult
    {
        public SyntaxTree SynataxTree { get; set; }

        public override string ToString()
        {
            return ((object)this.SynataxTree.RootNode ?? string.Empty).ToString();
        }
    }
}
