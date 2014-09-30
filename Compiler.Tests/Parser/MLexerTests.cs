namespace Compiler.Tests.Parser
{
    using System.IO;

    using Antlr4.Runtime;

    using Compiler.Parser.Antlr;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MLexerTests
    {
        [TestMethod]
        public void TestOperatorsWithSpace()
        {
            var sw = new StringReader("+ - * / ** %");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.Plus, tokens[0].Type);
            Assert.AreEqual(MLexer.Minus, tokens[1].Type);
            Assert.AreEqual(MLexer.Star, tokens[2].Type);
            Assert.AreEqual(MLexer.Div, tokens[3].Type);
            Assert.AreEqual(MLexer.StarStar, tokens[4].Type);
            Assert.AreEqual(MLexer.Mod, tokens[5].Type);

            Assert.AreEqual(7, tokens.Count);
        }

        [TestMethod]
        public void TestOperators()
        {
            var sw = new StringReader("+-*/**%");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.Plus, tokens[0].Type);
            Assert.AreEqual(MLexer.Minus, tokens[1].Type);
            Assert.AreEqual(MLexer.Star, tokens[2].Type);
            Assert.AreEqual(MLexer.Div, tokens[3].Type);
            Assert.AreEqual(MLexer.StarStar, tokens[4].Type);
            Assert.AreEqual(MLexer.Mod, tokens[5].Type);

            Assert.AreEqual(7, tokens.Count);
        }

        [TestMethod]
        public void TestBracketsWithSpace()
        {
            var sw = new StringReader(" [ ] { } ( ) ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.LeftBracket, tokens[0].Type);
            Assert.AreEqual(MLexer.RightBracket, tokens[1].Type);
            Assert.AreEqual(MLexer.LeftBrace, tokens[2].Type);
            Assert.AreEqual(MLexer.RightBrace, tokens[3].Type);
            Assert.AreEqual(MLexer.LeftParen, tokens[4].Type);
            Assert.AreEqual(MLexer.RightParen, tokens[5].Type);

            Assert.AreEqual(7, tokens.Count);
        }

        [TestMethod]
        public void TestBrackets()
        {
            var sw = new StringReader("[]{}()");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.LeftBracket, tokens[0].Type);
            Assert.AreEqual(MLexer.RightBracket, tokens[1].Type);
            Assert.AreEqual(MLexer.LeftBrace, tokens[2].Type);
            Assert.AreEqual(MLexer.RightBrace, tokens[3].Type);
            Assert.AreEqual(MLexer.LeftParen, tokens[4].Type);
            Assert.AreEqual(MLexer.RightParen, tokens[5].Type);

            Assert.AreEqual(7, tokens.Count);
        }

        [TestMethod]
        public void TestKeywords()
        {
            var sw = new StringReader("break void int if else for return new double string while bool");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.Break, tokens[0].Type);
            Assert.AreEqual(MLexer.Void, tokens[1].Type);
            Assert.AreEqual(MLexer.Int, tokens[2].Type);
            Assert.AreEqual(MLexer.If, tokens[3].Type);
            Assert.AreEqual(MLexer.Else, tokens[4].Type);
            Assert.AreEqual(MLexer.For, tokens[5].Type);
            Assert.AreEqual(MLexer.Return, tokens[6].Type);
            Assert.AreEqual(MLexer.New, tokens[7].Type);
            Assert.AreEqual(MLexer.Double, tokens[8].Type);
            Assert.AreEqual(MLexer.String, tokens[9].Type);
            Assert.AreEqual(MLexer.While, tokens[10].Type);
            Assert.AreEqual(MLexer.Bool, tokens[11].Type);

            Assert.AreEqual(13, tokens.Count);
        }

        [TestMethod]
        public void TestComparisons()
        {
            var sw = new StringReader("< <= > >= == != && ||");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.Less, tokens[0].Type);
            Assert.AreEqual(MLexer.LessEqual, tokens[1].Type);
            Assert.AreEqual(MLexer.Greater, tokens[2].Type);
            Assert.AreEqual(MLexer.GreaterEqual, tokens[3].Type);
            Assert.AreEqual(MLexer.Equal, tokens[4].Type);
            Assert.AreEqual(MLexer.NotEqual, tokens[5].Type);
            Assert.AreEqual(MLexer.AndAnd, tokens[6].Type);
            Assert.AreEqual(MLexer.OrOr, tokens[7].Type);

            Assert.AreEqual(9, tokens.Count);
        }

        [TestMethod]
        public void TestSymbols()
        {
            var sw = new StringReader("; , =");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.Semi, tokens[0].Type);
            Assert.AreEqual(MLexer.Comma, tokens[1].Type);
            Assert.AreEqual(MLexer.Assign, tokens[2].Type);

            Assert.AreEqual(4, tokens.Count);
        }

        [TestMethod]
        public void TestIdentifier()
        {
            var sw = new StringReader(" aftenposten ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.Identifier, tokens[0].Type);
            Assert.AreEqual("aftenposten", tokens[0].Text);

            Assert.AreEqual(2, tokens.Count);
        }

        [TestMethod]
        public void TestIntegerConstant()
        {
            var sw = new StringReader(" 9876543210 ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.IntegerConstant, tokens[0].Type);
            Assert.AreEqual("9876543210", tokens[0].Text);

            Assert.AreEqual(2, tokens.Count);
        }

        [TestMethod]
        public void TestStringConstant()
        {
            var sw = new StringReader(" \"aftenposten\" ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.StringConstant, tokens[0].Type);
            Assert.AreEqual("\"aftenposten\"", tokens[0].Text);

            Assert.AreEqual(2, tokens.Count);
        }

        [TestMethod]
        public void TestBooleanConstant()
        {
            var sw = new StringReader(" true false ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.BooleanConstant, tokens[0].Type);
            Assert.AreEqual(MLexer.BooleanConstant, tokens[1].Type);
            Assert.AreEqual("true", tokens[0].Text);
            Assert.AreEqual("false", tokens[1].Text);

            Assert.AreEqual(3, tokens.Count);
        }

        [TestMethod]
        public void TestDoubleConstant()
        {
            var sw = new StringReader(" 324.34234 ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.DoubleConstant, tokens[0].Type);
            Assert.AreEqual("324.34234", tokens[0].Text);

            Assert.AreEqual(2, tokens.Count);
        }

        [TestMethod]
        public void TestDoubleConstant1()
        {
            var sw = new StringReader(" 543.21 ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();
            Assert.AreEqual(MLexer.DoubleConstant, tokens[0].Type);
            Assert.AreEqual("543.21", tokens[0].Text);

            Assert.AreEqual(2, tokens.Count);
        }

        

        [TestMethod]
        public void TestWhitespace()
        {
            var sw = new StringReader("  \n ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();

            Assert.AreEqual(1, tokens.Count);
        }

        [TestMethod]
        public void TestComment()
        {
            var sw = new StringReader("  /* HAHA */ ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();

            Assert.AreEqual(1, tokens.Count);
        }

        [TestMethod]
        public void TestLineComment()
        {
            var sw = new StringReader("  // HAHA  ");

            var input = new AntlrInputStream(sw);
            var lexer = new MLexer(input);

            var tokenStream = new CommonTokenStream(lexer);
            tokenStream.Fill();

            var tokens = tokenStream.GetTokens();

            Assert.AreEqual(1, tokens.Count);
        }
    }
}