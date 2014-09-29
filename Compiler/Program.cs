namespace Compiler
{
    using System;

    using Compiler.Parser.Antlr;
    using Compiler.SemanticAnalysis;
    using Compiler.SyntaxTree;

    class Program
    {
        static void Main(string[] args)
        {
            Logger logger = new Logger();

            var antlerParser = new AntlrParser(logger);
//            var result = antlerParser.ParseProgram(
//@"void test(int d, int e) {
//    int[] f = new int[-2];
//    int g;
//   int[] y = test2();
//
//    bool h = true;
//    double[] sale = f;
//
//    test1(1,2,3);
//    test();
//
//    if(3==3) {
//        int test = 43;
//    }
//
//    while (3==2) {
//        int v;
//    }
//
//    for (44=3,3>4,3) {
//        int d;
//        break;
//    }
//}
//
//int[] test2() {
//
//}
//
//");

            var result = antlerParser.ParseProgram(
@"int main()
{
  int n;
  int c;
  int k;
  int space = 1;
 
  printf(""Enter number of rowsn"");
  scanf(""%d"", n);
 
  space = n - 1;
 
  for (k = 1; k <= n; k=k+1)
  {
    for (c = 1; c <= space; c=c+1)
      printf("" "");
 
    space=space -1;
 
    for (c = 1; c <= 2*k-1; c=c+1)
      printf(""*"");
 
    printf(""n"");
  }
 
  space = 1;
 
  for (k = 1; k <= n - 1; k=k+1)
  {
    for (c = 1; c <= space; c=c+1)
      printf("" "");
 
    space=space +1;
 
    for (c = 1 ; c <= 2*(n-k)-1; c=c+1)
      printf(""*"");
 
    printf(""n"");
  }
 
  return 0;
}");

            var printer = new TreePrinter();
            printer.PrintTree(result.SynataxTree);
            
            var semanticChecker = new SemanticChecker(logger);
            semanticChecker.RunCheck(result.SynataxTree);

            logger.PrintMessages();

            if (logger.TotalErrors > 0)
            {
                Console.WriteLine(
                    "Compilation failed, {0} errors {1} warnings, and {2} informationss",
                    logger.TotalErrors,
                    logger.TotalWarnings,
                    logger.TotalInfo);
            }

            Console.ReadLine();
        }
    }
}
