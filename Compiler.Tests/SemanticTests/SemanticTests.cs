namespace Compiler.Tests.SemanticTests
{
    using System.IO;

    using Compiler.Parser.Antlr;
    using Compiler.SemanticAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SemanticTests
    {
        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal01.m")]
        public void SemanticIllegal01Test()
        {
            var program = File.ReadAllText("SemanticIllegal01.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal02.m")]
        public void SemanticIllegal02Test()
        {
            var program = File.ReadAllText("SemanticIllegal02.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal03.m")]
        public void SemanticIllegal03Test()
        {
            var program = File.ReadAllText("SemanticIllegal03.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal04.m")]
        public void SemanticIllegal04Test()
        {
            var program = File.ReadAllText("SemanticIllegal04.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal05.m")]
        public void SemanticIllegal05Test()
        {
            var program = File.ReadAllText("SemanticIllegal05.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal06.m")]
        public void SemanticIllegal06Test()
        {
            var program = File.ReadAllText("SemanticIllegal06.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal07.m")]
        public void SemanticIllegal07Test()
        {
            var program = File.ReadAllText("SemanticIllegal07.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticIllegal08.m")]
        public void SemanticIllegal08Test()
        {
            var program = File.ReadAllText("SemanticIllegal08.m");
            Assert.IsFalse(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticLegal01.m")]
        public void SemanticLegal01Test()
        {
            var program = File.ReadAllText("SemanticLegal01.m");
            Assert.IsTrue(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticLegal02.m")]
        public void SemanticLegal02Test()
        {
            var program = File.ReadAllText("SemanticLegal02.m");
            Assert.IsTrue(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticLegal03.m")]
        public void SemanticLegal03Test()
        {
            var program = File.ReadAllText("SemanticLegal03.m");
            Assert.IsTrue(Parse(program));
        }

        [TestMethod]
        [DeploymentItem("SemanticTests/Files/SemanticLegal04.m")]
        public void SemanticLegal04Test()
        {
            var program = File.ReadAllText("SemanticLegal04.m");
            Assert.IsTrue(Parse(program));
        }

        private static bool Parse(string program)
        {
            var logger = new Logger();

            var antlerParser = new AntlrParser(logger);
            var tree = antlerParser.ParseProgram(program);

            var checker = new SemanticChecker(logger);
            checker.RunCheck(tree.SynataxTree);

            return logger.TotalErrors == 0;
        }
    }
}
