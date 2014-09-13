//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\mariusg\documents\visual studio 2013\Projects\Compiler\Compiler.Parser\Compiler.Parser\MParser.g4 by ANTLR 4.3

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

namespace Compiler.Parser {
using Antlr4.Runtime.Misc;
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="MParser"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public interface IMParserListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterExpression([NotNull] MParser.ExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.expression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitExpression([NotNull] MParser.ExpressionContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.functionDecleration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFunctionDecleration([NotNull] MParser.FunctionDeclerationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.functionDecleration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFunctionDecleration([NotNull] MParser.FunctionDeclerationContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.constant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterConstant([NotNull] MParser.ConstantContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.constant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitConstant([NotNull] MParser.ConstantContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterForStatement([NotNull] MParser.ForStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.forStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitForStatement([NotNull] MParser.ForStatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCall([NotNull] MParser.MethodCallContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.methodCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCall([NotNull] MParser.MethodCallContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.statementBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatementBlock([NotNull] MParser.StatementBlockContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.statementBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatementBlock([NotNull] MParser.StatementBlockContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterReturnStatement([NotNull] MParser.ReturnStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.returnStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitReturnStatement([NotNull] MParser.ReturnStatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] MParser.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] MParser.TypeContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.coreExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCoreExpression([NotNull] MParser.CoreExpressionContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.coreExpression"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCoreExpression([NotNull] MParser.CoreExpressionContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.creator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterCreator([NotNull] MParser.CreatorContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.creator"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitCreator([NotNull] MParser.CreatorContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterIfStatement([NotNull] MParser.IfStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.ifStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitIfStatement([NotNull] MParser.IfStatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStatement([NotNull] MParser.StatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStatement([NotNull] MParser.StatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.arguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterArguments([NotNull] MParser.ArgumentsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.arguments"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitArguments([NotNull] MParser.ArgumentsContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterWhileStatement([NotNull] MParser.WhileStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.whileStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitWhileStatement([NotNull] MParser.WhileStatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProgram([NotNull] MParser.ProgramContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.program"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProgram([NotNull] MParser.ProgramContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.breakStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterBreakStatement([NotNull] MParser.BreakStatementContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.breakStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitBreakStatement([NotNull] MParser.BreakStatementContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.variableDecleration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariableDecleration([NotNull] MParser.VariableDeclerationContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.variableDecleration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariableDecleration([NotNull] MParser.VariableDeclerationContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterParameters([NotNull] MParser.ParametersContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.parameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitParameters([NotNull] MParser.ParametersContext context);

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterVariable([NotNull] MParser.VariableContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.variable"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitVariable([NotNull] MParser.VariableContext context);
}
} // namespace Compiler.Parser
