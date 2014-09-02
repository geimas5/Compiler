using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Antlr4.Runtime;

namespace Compiler.Scanner.Tests
{
    [TestClass]
    public class MLexerTests
    {
        [TestMethod]
        public void TestOperators()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("+ - * / ** %");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(7, tokens.GetTokens().Count);
        }

        [TestMethod]
        public void TestBrackets()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("[]{}()");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(7, tokens.GetTokens().Count);
        }

        [TestMethod]
        public void TestKeywords()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("break class void int this if else for static return new double string");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(14, tokens.GetTokens().Count);
        }

        [TestMethod]
        public void TestComparisons()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("< <= > >= == !=");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(7, tokens.GetTokens().Count);
        }

        [TestMethod]
        public void TestSymbols()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("; : , . =");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(6, tokens.GetTokens().Count);
        }

        [TestMethod]
        public void TestIdentifier()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("aftenposten");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(2, tokens.GetTokens().Count);
        }

        [TestMethod]
        public void TestIntegerConstant()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("32323 -23232");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(3, tokens.GetTokens().Count);
        }

        [TestMethod]
        public void TestStringLiteral()
        {
            var stream = new MemoryStream();
            StringReader sw = new StringReader("\"aftenposten\"");

            AntlrInputStream input = new AntlrInputStream(sw);
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();

            Assert.AreEqual(2, tokens.GetTokens().Count);
        }
    }
}
