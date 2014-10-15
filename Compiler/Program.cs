namespace Compiler
{
    using System;
    using System.IO;

    class Program
    {
        static void Main(string[] args)
        {
            string program = @"int main()
{
	int d = 4;
	d = d - 1;
	d = d + 4;
	d = d / 5;
	d = d * 6;

	if(d == 6){
        PrintLine(""OK"");
    }

    return 0;
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
                }
            }

            Assembler.ExecutAssemble();

            Console.WriteLine("Compilation successful");
            Console.ReadLine();
        }
    }
}
