using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Antlr4.Runtime;
using Compiler.Scanner;
using Antlr4.Runtime.Tree;

namespace Compiler.Parser.Tests
{
    [TestClass]
    public class ParserTests : IMParserListener
    {
        [TestMethod]
        public void TestMethod1()
        {
            var stream = new MemoryStream();

            AntlrInputStream input = new AntlrInputStream(@"class test { }");
            MLexer lexer = new MLexer(input);

            CommonTokenStream tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            MParser parser = new MParser(tokens);
            var tree = parser.program();

            ParseTreeWalker walker = new ParseTreeWalker(); // create standard walker
            walker.Walk(this, tree); // initiate walk of tree with listener
        }

        public void EnterExpression(MParser.ExpressionContext context)
        {
            
        }

        public void ExitExpression(MParser.ExpressionContext context)
        {
            
        }

        public void EnterMember(MParser.MemberContext context)
        {
            
        }

        public void ExitMember(MParser.MemberContext context)
        {
            
        }

        public void EnterConstant(MParser.ConstantContext context)
        {
            
        }

        public void ExitConstant(MParser.ConstantContext context)
        {
            
        }

        public void EnterForStatement(MParser.ForStatementContext context)
        {
            
        }

        public void ExitForStatement(MParser.ForStatementContext context)
        {
            
        }

        public void EnterMethodCall(MParser.MethodCallContext context)
        {
            
        }

        public void ExitMethodCall(MParser.MethodCallContext context)
        {
            
        }

        public void EnterReturnStatement(MParser.ReturnStatementContext context)
        {
            
        }

        public void ExitReturnStatement(MParser.ReturnStatementContext context)
        {
            
        }

        public void EnterStatementBlock(MParser.StatementBlockContext context)
        {
            
        }

        public void ExitStatementBlock(MParser.StatementBlockContext context)
        {
            
        }

        public void EnterType(MParser.TypeContext context)
        {
            
        }

        public void ExitType(MParser.TypeContext context)
        {
            
        }

        public void EnterPrototype(MParser.PrototypeContext context)
        {
            
        }

        public void ExitPrototype(MParser.PrototypeContext context)
        {
            
        }

        public void EnterCoreExpression(MParser.CoreExpressionContext context)
        {
            
        }

        public void ExitCoreExpression(MParser.CoreExpressionContext context)
        {
            
        }

        public void EnterCreator(MParser.CreatorContext context)
        {
            
        }

        public void ExitCreator(MParser.CreatorContext context)
        {
            
        }

        public void EnterIfStatement(MParser.IfStatementContext context)
        {
            
        }

        public void ExitIfStatement(MParser.IfStatementContext context)
        {
            
        }

        public void EnterMethodDecleration(MParser.MethodDeclerationContext context)
        {
            
        }

        public void ExitMethodDecleration(MParser.MethodDeclerationContext context)
        {
            
        }

        public void EnterStatement(MParser.StatementContext context)
        {
            
        }

        public void ExitStatement(MParser.StatementContext context)
        {
            
        }

        public void EnterArguments(MParser.ArgumentsContext context)
        {
            
        }

        public void ExitArguments(MParser.ArgumentsContext context)
        {
            
        }

        public void EnterWhileStatement(MParser.WhileStatementContext context)
        {
            
        }

        public void ExitWhileStatement(MParser.WhileStatementContext context)
        {
            
        }

        public void EnterProgram(MParser.ProgramContext context)
        {
            
        }

        public void ExitProgram(MParser.ProgramContext context)
        {
            
        }

        public void EnterClassDecleration(MParser.ClassDeclerationContext context)
        {
            
        }

        public void ExitClassDecleration(MParser.ClassDeclerationContext context)
        {
            
        }

        public void EnterInterfaceDecleration(MParser.InterfaceDeclerationContext context)
        {
            
        }

        public void ExitInterfaceDecleration(MParser.InterfaceDeclerationContext context)
        {
            
        }

        public void EnterBreakStatement(MParser.BreakStatementContext context)
        {
            
        }

        public void ExitBreakStatement(MParser.BreakStatementContext context)
        {
            
        }

        public void EnterVariableDecleration(MParser.VariableDeclerationContext context)
        {
            
        }

        public void ExitVariableDecleration(MParser.VariableDeclerationContext context)
        {
            
        }

        public void EnterParameters(MParser.ParametersContext context)
        {
            
        }

        public void ExitParameters(MParser.ParametersContext context)
        {
            
        }

        public void EnterVariable(MParser.VariableContext context)
        {
            
        }

        public void ExitVariable(MParser.VariableContext context)
        {
            
        }

        public void EnterEveryRule(ParserRuleContext ctx)
        {
            
        }

        public void ExitEveryRule(ParserRuleContext ctx)
        {
            
        }

        public void VisitErrorNode(IErrorNode node)
        {
            
        }

        public void VisitTerminal(ITerminalNode node)
        {
            
        }
    }
}
