namespace Compiler
{
    using System;
    using System.IO;

    using Compiler.Optimization;

    class Program
    {
        private static void Main(string[] args)
        {
            string program = @"int main()
{
    int d = Fibonacci(10);

    if(d == 55){
        PrintLine(""OK"");
    }

    return 0;
}

int Fibonacci(int n)
{
   if ( n == 0 )
      return 0;
   else if ( n == 1 )
      return 1;
   
   return ( Fibonacci(n-1) + Fibonacci(n-2) );
} ";

//            string program = @"int main()
//            {
//               int d = 34;
//               int f = 42;
//               d = 43 + f;
//               return d;
//            }
//          ";
            var asembly = new CompilerAssembly
                              {
                                  PrintMessages = true,
                                  PrintIR = true,
                                  ActivatedOptimizations =
                                      {
                                          Optimizations.EliminateEqualAssignments, 
                                          Optimizations.LocalCopyPropagation,
                                          Optimizations.DeadCodeElimination
                                      }
                              };

            using (var input = new StringReader(program))
            using (var outputStream = File.Create("output.asm"))
            using (var outputWriter = new StreamWriter(outputStream))
            {
                var successful = asembly.CompileProgram(input, outputWriter);

                if (!successful)
                {
                    Console.WriteLine(
                        "Compilation failed, {0} errors {1} warnings, and {2} information",
                        asembly.Logger.TotalErrors,
                        asembly.Logger.TotalWarnings,
                        asembly.Logger.TotalInfo);

                    Console.ReadLine();
                    return;
                }
            }

            Assembler.ExecutAssemble();

            Console.WriteLine("Compilation successful");
            Console.ReadLine();
        }
    }
}
