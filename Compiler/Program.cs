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
    PrintBool(true == true);PrintLine("""");
    PrintBool(false == true);PrintLine("""");
    PrintBool(true == false);PrintLine("""");
    PrintBool(false == false);PrintLine("""");

    PrintLine(""----------"");
    PrintBool(true != true);PrintLine("""");
    PrintBool(false != true);PrintLine("""");
    PrintBool(true != false);PrintLine("""");
    PrintBool(false != false);PrintLine("""");

    PrintLine(""----------"");
    PrintBool(!true);PrintLine("""");
    PrintBool(!false);PrintLine("""");

    PrintLine(""----------"");
    PrintBool(true && true); PrintLine("""");
    PrintBool(true && false); PrintLine("""");
    PrintBool(false && true); PrintLine("""");
    PrintBool(false && false); PrintLine("""");

    PrintLine(""----------"");
    PrintBool(true || true); PrintLine("""");
    PrintBool(true || false); PrintLine("""");
    PrintBool(false || true); PrintLine("""");
    PrintBool(false || false); PrintLine("""");

    PrintBool(GetTrue() == GetTrue()); PrintLine("""");
    PrintBool(GetFalse() == GetTrue()); PrintLine("""");
    PrintBool(GetTrue() == GetFalse()); PrintLine("""");
    PrintBool(GetFalse() == GetFalse()); PrintLine("""");

    PrintLine(""----------"");
    PrintBool(GetTrue() != GetTrue()); PrintLine("""");
    PrintBool(GetFalse() != GetTrue()); PrintLine("""");
    PrintBool(GetTrue() != GetFalse()); PrintLine("""");
    PrintBool(GetFalse() != GetFalse()); PrintLine("""");

    PrintLine(""----------"");
    PrintBool(!GetTrue()); PrintLine("""");
    PrintBool(!GetFalse()); PrintLine("""");

    PrintLine(""----------"");
    PrintBool(GetTrue() && GetTrue());PrintLine("""");
    PrintBool(GetTrue() && GetFalse());PrintLine("""");
    PrintBool(GetFalse() && GetTrue());PrintLine("""");
    PrintBool(GetFalse() && GetFalse());PrintLine("""");

    PrintLine(""----------"");
    PrintBool(GetTrue() || GetTrue());PrintLine("""");
    PrintBool(GetTrue() || GetFalse());PrintLine("""");
    PrintBool(GetFalse() || GetTrue());PrintLine("""");
    PrintBool(GetFalse() || GetFalse());PrintLine("""");
    
    return 0;
}

void PrintBool(bool val) {
    if(val){
        PrintInt(1);
    }
    else{
        PrintInt(0);
    }
}

bool GetTrue(){
    PrintBool(true);
    return true;
}

bool GetFalse(){
    PrintBool(false);
    return false;
}

";

            var asembly = new CompilerAssembly
                              {
                                  PrintMessages = true,
                                  PrintIR = true,
                                  OutputIr = true,
                                  AllocateRegisters = true,
                                  ActivatedOptimizations =
                                      {
                                          Optimizations.EliminateEqualAssignments, 
                                          Optimizations.LocalCopyPropagation,
                                          Optimizations.DeadCodeElimination,
                                          Optimizations.AlgebraicOptimization
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
