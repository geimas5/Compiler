namespace Compiler.Tests.Compiling
{
    using System.Diagnostics;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("Assemble.bat")]
    public class CompilingTests
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

        private void TestProgram(string programFile, string resultFile)
        {
            var result = this.CompileAndRunProgram(programFile);
            Assert.AreEqual(File.ReadAllText(resultFile), result);
        }

        private string CompileAndRunProgram(string file)
        {
            var asembly = new CompilerAssembly();

            using (var input = new StringReader(File.ReadAllText(file)))
            using (var outputStream = File.Create("output.asm"))
            using (var outputWriter = new StreamWriter(outputStream))
            {
                var successful = asembly.CompileProgram(input, outputWriter);
                Assert.IsTrue(successful);
            }

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
