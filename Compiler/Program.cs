namespace Compiler
{
    using System;
    using System.IO;

    using Compiler.Optimization;

    class Program
    {
        private static void Main(string[] args)
        {
//            string program = @"int main()
//            {
//                int[][] d = new int[6][6];
//
//                d[0][5] = 1;
//                d[1][4] = 2;
//                d[2][3] = 3;
//                d[3][2] = 4;
//                d[4][1] = 5;
//                d[5][0] = 6;       
//               
//                int i;
//                int j;
//                int c = 0;
//                for(i = 0; i < 6; i = i + 1) {
//                    for(j = 0; j < 6; j = j + 1) {
//                        PrintLine(""ergeg"");
//                        c=c+1;
//                    }
//                }
//                PrintInt(c);
//
//               return 1;
//            }";

            string program = @"int main()
{
    double[] v = new double[5];
    v[0] = 443 + 23;
    v[1] = 32 - 223;
    v[2] = 23 * 34;
    v[3] = 453 / 32;
    v[4] = 23 ** 5;

    PrintDouble(v[0]); PrintLine("""");
    PrintDouble(v[1]); PrintLine("""");
    PrintDouble(v[2]); PrintLine("""");
    PrintDouble(v[3]); PrintLine("""");
    PrintDouble(v[4]); PrintLine("""");
    
    return 0;
}

//void PrintBool(bool val) {
//    if(val){
//        PrintInt(1);
//    }
//    else{
//        PrintInt(0);
//    }
//}
//
//bool GetTrue(){
//    PrintBool(true);
//    return true;
//}
//
//bool GetFalse(){
//    PrintBool(false);
//    return false;
//}

";

            var asembly = new CompilerAssembly
                              {
                                  PrintMessages = true,
                                  PrintIR = true,
                                  OutputIr = true,
                                  AllocateRegisters = true,
                                  ActivatedOptimizations =
                                      {
                                          //Optimizations.EliminateEqualAssignments, 
                                          //Optimizations.LocalCopyPropagation,
                                          //Optimizations.DeadCodeElimination,
                                          //Optimizations.AlgebraicOptimization
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
