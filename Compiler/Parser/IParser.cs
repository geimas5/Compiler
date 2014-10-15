
namespace Compiler.Parser
{
    public interface IParser
    {
        ParsingResult ParseProgram(string program);
    }
}
