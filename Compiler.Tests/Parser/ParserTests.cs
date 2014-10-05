namespace Compiler.Tests.Parser
{
    using System.IO;

    using Compiler.Parser.Antlr;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal01.m")]
        public void ParserIllegal01Test()
        {
            var program = File.ReadAllText("ParserIllegal01.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal02.m")]
        public void ParserIllegal02Test()
        {
            var program = File.ReadAllText("ParserIllegal02.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal03.m")]
        public void ParserIllegal03Test()
        {
            var program = File.ReadAllText("ParserIllegal03.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal04.m")]
        public void ParserIllegal04Test()
        {
            var program = File.ReadAllText("ParserIllegal04.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal05.m")]
        public void ParserIllegal05Test()
        {
            var program = File.ReadAllText("ParserIllegal05.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal06.m")]
        public void ParserIllegal06Test()
        {
            var program = File.ReadAllText("ParserIllegal06.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal07.m")]
        public void ParserIllegal07Test()
        {
            var program = File.ReadAllText("ParserIllegal07.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal08.m")]
        public void ParserIllegal08Test()
        {
            var program = File.ReadAllText("ParserIllegal08.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal09.m")]
        public void ParserIllegal09Test()
        {
            var program = File.ReadAllText("ParserIllegal09.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserIllegal10.m")]
        public void ParserIllegal10Test()
        {
            var program = File.ReadAllText("ParserIllegal10.m");
            Assert.IsFalse(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal01.m")]
        public void ParserLegal01Test()
        {
            var program = File.ReadAllText("ParserLegal01.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal02.m")]
        public void ParserLegal02Test()
        {
            var program = File.ReadAllText("ParserLegal02.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal03.m")]
        public void ParserLegal03Test()
        {
            var program = File.ReadAllText("ParserLegal03.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal04.m")]
        public void ParserLegal04Test()
        {
            var program = File.ReadAllText("ParserLegal04.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal05.m")]
        public void ParserLegal05Test()
        {
            var program = File.ReadAllText("ParserLegal05.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal06.m")]
        public void ParserLegal06Test()
        {
            var program = File.ReadAllText("ParserLegal06.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal07.m")]
        public void ParserLegal07Test()
        {
            var program = File.ReadAllText("ParserLegal07.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal08.m")]
        public void ParserLegal08Test()
        {
            var program = File.ReadAllText("ParserLegal08.m");
            Assert.IsTrue(this.Parse(program));
        }

        [TestMethod]
        [DeploymentItem("Parser/Parsing/ParserLegal09.m")]
        public void ParserLegal09Test()
        {
            var program = File.ReadAllText("ParserLegal09.m");
            Assert.IsTrue(this.Parse(program));
        }

        private bool Parse(string program)
        {
            var logger = new Logger();

            var antlerParser = new AntlrParser(logger);
            antlerParser.ParseProgram(program);

            return logger.TotalErrors == 0;
        }
    }
}
