namespace Compiler
{
    using System;

    using Compiler.Parser.Antlr;

    class Program
    {
        static void Main(string[] args)
        {
            var antlerParser = new AntlrParser();
            var result = antlerParser.ParseProgram(
@"

void test() {
    int[] d = new int[2];

    test(1,2,3);
    test();

    if(3=3) {
        test = 43;
    }

    while (3=2) {
        int v;
    }

    for (44=3,3>4,3) {
        int d;
    }
}

int[] test() {

}

");

            Console.WriteLine(result.SynataxTree.ToString());
            Console.ReadLine();
        }
    }
}
