using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace Compiler.Parser.Tests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Program_classWithInheritanceChild()
        {
            var stream = new MemoryStream();

            AntlrInputStream input = new AntlrInputStream(@"class test : test2 { void method(){}; }");
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MParser parser = new MParser(tokens);
            var tree = parser.program();

            Assert.IsInstanceOfType(tree.children[0], typeof(MParser.ClassDeclerationContext));
            Assert.IsInstanceOfType(tree.children[0].GetChild(5), typeof(MParser.MemberContext));
        }

        [TestMethod]
        public void Program_classChild()
        {
            var stream = new MemoryStream();

            AntlrInputStream input = new AntlrInputStream(@"class test { void method(){}; }");
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MParser parser = new MParser(tokens);
            var tree = parser.program();

            Assert.IsInstanceOfType(tree.children[0], typeof(MParser.ClassDeclerationContext));
            Assert.IsInstanceOfType(tree.children[0].GetChild(3), typeof(MParser.MemberContext));
        }

        [TestMethod]
        public void Program_InterfaceChild()
        {
            var stream = new MemoryStream();

            AntlrInputStream input = new AntlrInputStream(@"interface test { void method(); }");
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MParser parser = new MParser(tokens);
            var tree = parser.program();

            Assert.IsInstanceOfType(tree.children[0], typeof(MParser.InterfaceDeclerationContext));
            Assert.IsInstanceOfType(tree.children[0].GetChild(3), typeof(MParser.PrototypeContext));
        }

        [TestMethod]
        public void member_methodDecleration()
        {
            var stream = new MemoryStream();

            AntlrInputStream input = new AntlrInputStream(@"void method() { int d; } }");
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MParser parser = new MParser(tokens);
            var tree = parser.member();

            Assert.IsInstanceOfType(tree.children[0], typeof(MParser.MethodDeclerationContext));
            Assert.IsInstanceOfType(tree.children[0].GetChild(4), typeof(MParser.StatementBlockContext));
        }


        [TestMethod]
        public void member_variableDecleration()
        {
            var stream = new MemoryStream();

            AntlrInputStream input = new AntlrInputStream(@"int d;");
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            MParser parser = new MParser(tokens);
            var tree = parser.member();

            Assert.IsInstanceOfType(tree.children[0], typeof(MParser.VariableDeclerationContext));
        }
    }
}
