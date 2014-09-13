namespace Compiler.Parser
{
    using System.Collections.Generic;

    using Compiler.SyntaxTree;

    public class ParsingResult
    {
        public SyntaxTree SynataxTree { get; set; }

        public List<Error> Errors { get; set; }

        public override string ToString()
        {
            return ((object)this.SynataxTree.RootNode ?? string.Empty).ToString();
        }
    }
}
