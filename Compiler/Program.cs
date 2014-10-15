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
            var asembly = new CompilerAssembly();

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

            //string d = "set LINKCMD64=C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\bin\amd64\link.exe";
            //var assebleBat = @"""C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\bin\amd64\ml64"" output.asm /link /subsystem:console /defaultlib:""C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Lib\x64\Kernel32.Lib"" /entry:main";
            //File.WriteAllText("Assemble.bat", assebleBat);

            // ml64 output.asm /link /subsystem:console /defaultlib:"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Lib\x64\Kernel32.Lib" /entry:main
        }
    }
}
