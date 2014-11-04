namespace Compiler.Tests.Compiling
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using Compiler.Optimization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("Assemble.bat")]
    public class RegisterAllocationFullOptimizationTests
    {
        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program1.m")]
        [DeploymentItem("Compiling/Programs/Program1Result.txt")]
        public void TestProgram1()
        {
            this.TestProgram("Program1.m", "Program1Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program2.m")]
        [DeploymentItem("Compiling/Programs/Program2Result.txt")]
        public void TestProgram2()
        {
            this.TestProgram("Program2.m", "Program2Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program3.m")]
        [DeploymentItem("Compiling/Programs/Program3Result.txt")]
        public void TestProgram3()
        {
            this.TestProgram("Program3.m", "Program3Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program4.m")]
        [DeploymentItem("Compiling/Programs/Program4Result.txt")]
        public void TestProgram4()
        {
            this.TestProgram("Program4.m", "Program4Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program5.m")]
        [DeploymentItem("Compiling/Programs/Program5Result.txt")]
        public void TestProgram5()
        {
            this.TestProgram("Program5.m", "Program5Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program6.m")]
        [DeploymentItem("Compiling/Programs/Program6Result.txt")]
        public void TestProgram6()
        {
            this.TestProgram("Program6.m", "Program6Result.txt");
        }

        private void TestProgram(string programFile, string resultFile)
        {
            var result = this.CompileAndRunProgram(programFile).Trim();
            Assert.AreEqual(File.ReadAllText(resultFile).Trim(), result);
        }

        private string CompileAndRunProgram(string file)
        {
            File.Delete("output.asm");
            File.Delete("output.exe");

            var asembly = new CompilerAssembly
                              {
                                  ActivatedOptimizations =
                                      {
                                          Optimizations.AlgebraicOptimization,
                                          Optimizations.DeadCodeElimination,
                                          Optimizations.EliminateEqualAssignments,
                                          Optimizations.LocalCopyPropagation,
                                      },
                                  AllocateRegisters = true
                              };

            using (var input = new StringReader(File.ReadAllText(file)))
            using (var outputStream = File.Create("output.asm"))
            using (var outputWriter = new StreamWriter(outputStream))
            {
                var successful = asembly.CompileProgram(input, outputWriter);
                Assert.IsTrue(successful);
            }
            Thread.Sleep(100);

            Assembler.ExecutAssemble();

            return this.RunProgram();
        }

        private string RunProgram()
        {
            var procStartInfo = new ProcessStartInfo("output.exe")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };


            var proc = new Process { StartInfo = procStartInfo };
            proc.Start();
            return proc.StandardOutput.ReadToEnd();
        }
    }
}
