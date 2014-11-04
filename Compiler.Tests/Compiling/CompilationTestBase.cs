namespace Compiler.Tests.Compiling
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public abstract class CompilationTestBase
    {
        private string OutputPrefix;

        public CompilationTestBase(string outputPrefix)
        {
            this.OutputPrefix = outputPrefix;
        }

        protected void TestProgram(string programFile, string resultFile)
        {
            var result = this.CompileAndRunProgram(programFile).Trim();
            Assert.AreEqual(File.ReadAllText(resultFile).Trim(), result);
        }

        private string CompileAndRunProgram(string file)
        {
            File.Delete(OutputPrefix + file + "output.asm");
            File.Delete(OutputPrefix + file + "output.exe");

            var asembly = this.CreateCompilerAssembly();

            using (var input = new StringReader(File.ReadAllText(file)))
            using (var outputStream = File.Create(OutputPrefix + file + "output.asm"))
            using (var outputWriter = new StreamWriter(outputStream))
            {
                var successful = asembly.CompileProgram(input, outputWriter);
                Assert.IsTrue(successful);
                outputWriter.Flush();
                outputStream.Flush(true);
            }

            Thread.Sleep(100);

            Assembler.ExecutAssemble(OutputPrefix + file + "output.asm");

            return this.RunProgram(file);
        }

        private string RunProgram(string file)
        {
            var procStartInfo = new ProcessStartInfo(OutputPrefix + file + "output.exe")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };


            var proc = new Process { StartInfo = procStartInfo };
            proc.Start();
            return proc.StandardOutput.ReadToEnd();
        }

        protected abstract CompilerAssembly CreateCompilerAssembly();
    }
}
