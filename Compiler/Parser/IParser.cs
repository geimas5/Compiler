
namespace Compiler.Parser
{
    using System.IO;

    public interface IParser
    {
        ParsingResult ParseProgram(string program);
    }
}
