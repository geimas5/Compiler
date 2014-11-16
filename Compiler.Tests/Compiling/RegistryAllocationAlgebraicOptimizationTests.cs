namespace Compiler.Tests.Compiling
{
    using Compiler.Optimization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DeploymentItem("Assemble.bat")]
    public class RegistryAllocationAlgebraicOptimizationTests : CompilationTestBase
    {
        public RegistryAllocationAlgebraicOptimizationTests()
            : base("RegistryAllocationAlgebraicOptimization")
        {
        }

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

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program7.m")]
        [DeploymentItem("Compiling/Programs/Program7Result.txt")]
        public void TestProgram7()
        {
            this.TestProgram("Program7.m", "Program7Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program8.m")]
        [DeploymentItem("Compiling/Programs/Program8Result.txt")]
        public void TestProgram8()
        {
            this.TestProgram("Program8.m", "Program8Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program9.m")]
        [DeploymentItem("Compiling/Programs/Program9Result.txt")]
        public void TestProgram9()
        {
            this.TestProgram("Program9.m", "Program9Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program10.m")]
        [DeploymentItem("Compiling/Programs/Program10Result.txt")]
        public void TestProgram10()
        {
            this.TestProgram("Program10.m", "Program10Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program11.m")]
        [DeploymentItem("Compiling/Programs/Program11Result.txt")]
        public void TestProgram11()
        {
            this.TestProgram("Program11.m", "Program11Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program12.m")]
        [DeploymentItem("Compiling/Programs/Program12Result.txt")]
        public void TestProgram12()
        {
            this.TestProgram("Program12.m", "Program12Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program13.m")]
        [DeploymentItem("Compiling/Programs/Program13Result.txt")]
        public void TestProgram13()
        {
            this.TestProgram("Program13.m", "Program13Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program14.m")]
        [DeploymentItem("Compiling/Programs/Program14Result.txt")]
        public void TestProgram14()
        {
            this.TestProgram("Program14.m", "Program14Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program15.m")]
        [DeploymentItem("Compiling/Programs/Program15Result.txt")]
        public void TestProgram15()
        {
            this.TestProgram("Program15.m", "Program15Result.txt");
        }

        [TestMethod]
        [DeploymentItem("Compiling/Programs/Program16.m")]
        [DeploymentItem("Compiling/Programs/Program16Result.txt")]
        public void TestProgram16()
        {
            this.TestProgram("Program16.m", "Program16Result.txt");
        }

        protected override CompilerAssembly CreateCompilerAssembly()
        {
            return new CompilerAssembly
                       {
                           ActivatedOptimizations = { Optimizations.AlgebraicOptimization },
                           AllocateRegisters = true
                       };
        }
    }
}
