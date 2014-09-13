namespace Compiler.Parser.Antlr
{
    using System;
    using System.Collections.Generic;
    using System.Data.Odbc;
    using System.Runtime.CompilerServices;

    using Antlr4.Runtime;
    using Antlr4.Runtime.Tree;

    using Compiler.SyntaxTree;

    //public class ParseTreeListener  : IMParserListener
    //{
    //    private readonly SyntaxTree syntaxTree = new SyntaxTree();
    //    private readonly List<Error> errorMessages = new List<Error>();
    //    private readonly Stack<Node> nodeStack = new Stack<Node>(); 


    //    public ParsingResult GetParsingResult()
    //    {
    //        return new ParsingResult { Errors = this.errorMessages, SynataxTree = this.syntaxTree };
    //    }

    //    public void VisitTerminal(ITerminalNode node)
    //    {
    //    }

    //    public void VisitErrorNode(IErrorNode node)
    //    {
    //        this.errorMessages.Add(new Error { ErrorMessage = node.ToString() });
    //    }

    //    public void EnterEveryRule(ParserRuleContext ctx)
    //    {
    //    }

    //    public void ExitEveryRule(ParserRuleContext ctx)
    //    {
    //    }

    //    public void EnterExpression(MParser.ExpressionContext context)
    //    {
    //    }

    //    public void ExitExpression(MParser.ExpressionContext context)
    //    {
    //    }

    //    public void EnterFunctionDecleration(MParser.FunctionDeclerationContext context)
    //    {
    //        var functionNode = new FunctionDecleration(this.CreateLocation(context));
    //        ((ProgramNode)this.nodeStack.Peek()).Functions.Add(functionNode);

    //        functionNode.Name = context.GetToken(MParser.Identifier, 0).GetText();

    //        this.nodeStack.Push(functionNode);
    //    }

    //    public void ExitFunctionDecleration(MParser.FunctionDeclerationContext context)
    //    {
    //        this.nodeStack.Pop();
    //    }

    //    public void EnterConstant(MParser.ConstantContext context)
    //    {   
    //    }

    //    public void ExitConstant(MParser.ConstantContext context)
    //    {   
    //    }

    //    public void EnterForStatement(MParser.ForStatementContext context)
    //    {   
            
    //    }

    //    public void ExitForStatement(MParser.ForStatementContext context)
    //    {
    //    }

    //    public void EnterMethodCall(MParser.MethodCallContext context)
    //    {   
    //    }

    //    public void ExitMethodCall(MParser.MethodCallContext context)
    //    {   
    //    }

    //    public void EnterStatementBlock(MParser.StatementBlockContext context)
    //    {
    //        var blockStatement = new BlockStatement(this.CreateLocation(context));

    //        if (this.nodeStack.Peek() is FunctionDecleration)
    //        {
    //            ((FunctionDecleration)this.nodeStack.Peek()).Block = blockStatement;
    //        }
    //        else
    //        {
    //            throw new Exception();
    //        }

    //        this.nodeStack.Push(blockStatement);
    //    }

    //    public void ExitStatementBlock(MParser.StatementBlockContext context)
    //    {
    //        this.nodeStack.Pop();
    //    }

    //    public void EnterReturnStatement(MParser.ReturnStatementContext context)
    //    {   
    //    }

    //    public void ExitReturnStatement(MParser.ReturnStatementContext context)
    //    {   
    //    }

    //    public void EnterType(MParser.TypeContext context)
    //    {
    //    }

    //    public void ExitType(MParser.TypeContext context)
    //    {
    //        var type = ParseType(context);

    //        this.nodeStack.Push(new TypeNode(this.CreateLocation(context))
    //                                {
    //                                    Type = type
    //                                });
    //    }

    //    public void EnterCoreExpression(MParser.CoreExpressionContext context)
    //    {    
    //    }

    //    public void ExitCoreExpression(MParser.CoreExpressionContext context)
    //    {   
    //    }

    //    public void EnterCreator(MParser.CreatorContext context)
    //    {

    //    }

    //    public void ExitCreator(MParser.CreatorContext context)
    //    {   
    //    }

    //    public void EnterIfStatement(MParser.IfStatementContext context)
    //    {   
    //    }

    //    public void ExitIfStatement(MParser.IfStatementContext context)
    //    {   
    //    }

    //    public void EnterStatement(MParser.StatementContext context)
    //    {   
    //    }

    //    public void ExitStatement(MParser.StatementContext context)
    //    {   
    //    }

    //    public void EnterArguments(MParser.ArgumentsContext context)
    //    {
    //    }

    //    public void ExitArguments(MParser.ArgumentsContext context)
    //    {   
    //    }

    //    public void EnterWhileStatement(MParser.WhileStatementContext context)
    //    {   
    //    }

    //    public void ExitWhileStatement(MParser.WhileStatementContext context)
    //    {   
    //    }

    //    public void EnterProgram(MParser.ProgramContext context)
    //    {
    //        var programNode = new ProgramNode(this.CreateLocation(context));

    //        this.syntaxTree.RootNode = programNode;
    //        this.nodeStack.Push(programNode);
    //    }

    //    public void ExitProgram(MParser.ProgramContext context)
    //    {
    //        this.nodeStack.Pop();
    //    }

    //    public void EnterBreakStatement(MParser.BreakStatementContext context)
    //    {   
    //    }

    //    public void ExitBreakStatement(MParser.BreakStatementContext context)
    //    {   
    //    }

    //    public void EnterVariableDecleration(MParser.VariableDeclerationContext context)
    //    {
    //    }

    //    public void ExitVariableDecleration(MParser.VariableDeclerationContext context)
    //    {
    //        var variableDecleration = new VariableDecleration(this.CreateLocation(context));
    //        variableDecleration.Name = new VariableIdNode(this.CreateLocation(context.GetToken(MParser.Identifier, 0)))
    //                                       {
    //                                           Name = context.GetToken(MParser.Identifier, 0).GetText()
    //                                       };

    //        variableDecleration.Type = (TypeNode)this.nodeStack.Pop();

    //        if (this.nodeStack.Peek() is BlockStatement)
    //        {
    //            var block = (BlockStatement)this.nodeStack.Peek();
    //            block.Statements.Add(variableDecleration);
    //        }
    //    }

    //    public void EnterParameters(MParser.ParametersContext context)
    //    {
    //    }

    //    public void ExitParameters(MParser.ParametersContext context)
    //    {   
    //    }

    //    public void EnterVariable(MParser.VariableContext context)
    //    {
    //    }

    //    public void ExitVariable(MParser.VariableContext context)
    //    {
    //        var variable = new VariableNode(this.CreateLocation(context));
    //        var type = (TypeNode)this.nodeStack.Pop();
    //        variable.Type = type;
    //        variable.Name = new VariableIdNode(this.CreateLocation(context.GetToken(MParser.Identifier, 0)));

    //        if (this.nodeStack.Peek() is FunctionDecleration)
    //        {
    //            ((FunctionDecleration)this.nodeStack.Peek()).Parameters.Add(variable);
    //        }
    //        else
    //        {
    //            throw new Exception("Should not happen");
    //        }
    //    }

    //    private Location CreateLocation(ParserRuleContext context)
    //    {
    //        return new Location(context.Start.Column, context.Start.Line);
    //    }

    //    private Location CreateLocation(ITerminalNode node)
    //    {
    //        return new Location(node.Symbol.Column, node.Symbol.Line);
    //    }

    //    private static Types ParseType(MParser.TypeContext context)
    //    {
    //        Types type;

    //        switch (context.GetText())
    //        {
    //            case "int":
    //                type = Types.Int;
    //                break;
    //            case "double":
    //                type = Types.Double;
    //                break;
    //            case "string":
    //                type = Types.String;
    //                break;
    //            default:
    //                throw new Exception();
    //        }
    //        return type;
    //    }
    //}
}
