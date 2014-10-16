namespace Compiler
{
    using System;
    using System.IO;

    class Program
    {
        private static void Main(string[] args)
        {
            string program = @"int main() {
   double d = 3.5;
   d = d - 10.5;
   print(d);
   return 0;
}

void print(double e){
    PrintDouble(e);
}";
            var asembly = new CompilerAssembly { PrintMessages = true };

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
