namespace Compiler
{
    using System;
    using System.IO;

    using Compiler.Optimization;

    class Program
    {
        private static void Main(string[] args)
        {
            string program = @"int main(){
   int r;
   int number = 1000;
   int c;
   int sum = 0;
   int temp;

   for( c = 1 ; c <= number ; c = c + 1 )
   {
      temp = c;
      while( temp != 0 )
      {
         r = temp % 10;
         sum = sum + r * r * r;
         temp = temp / 10;
      }

      if ( c == sum ){
        PrintInt(c);
        PrintLine("""");
      }

      sum = 0;
   }
 
   return 0;
}";
            var asembly = new CompilerAssembly
                              {
                                  PrintMessages = true,
                                  PrintIR = true,
                                  ActivatedOptimizations = { Optimizations.EliminateEqualAssignments }
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
