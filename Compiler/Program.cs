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
    double d = 33.1*3;

    PrintDouble(d);
 
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
