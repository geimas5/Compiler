//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Users\mariusg\documents\visual studio 2013\Projects\Compiler\Compiler\Parser\Antlr\MParser.g4 by ANTLR 4.3

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

namespace Compiler.Parser.Antlr {
 
	using Compiler.SyntaxTree; 


using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="IMParserListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public partial class MParserBaseListener : IMParserListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExpression([NotNull] MParser.ExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.expression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExpression([NotNull] MParser.ExpressionContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.functionDecleration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionDecleration([NotNull] MParser.FunctionDeclerationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.functionDecleration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionDecleration([NotNull] MParser.FunctionDeclerationContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.statementOrBlock"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatementOrBlock([NotNull] MParser.StatementOrBlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.statementOrBlock"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatementOrBlock([NotNull] MParser.StatementOrBlockContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.constant"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterConstant([NotNull] MParser.ConstantContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.constant"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitConstant([NotNull] MParser.ConstantContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.forStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterForStatement([NotNull] MParser.ForStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.forStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitForStatement([NotNull] MParser.ForStatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterReturnStatement([NotNull] MParser.ReturnStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.returnStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitReturnStatement([NotNull] MParser.ReturnStatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.statementBlock"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatementBlock([NotNull] MParser.StatementBlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.statementBlock"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatementBlock([NotNull] MParser.StatementBlockContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.creatorSizes"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCreatorSizes([NotNull] MParser.CreatorSizesContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.creatorSizes"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCreatorSizes([NotNull] MParser.CreatorSizesContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.type"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterType([NotNull] MParser.TypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.type"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitType([NotNull] MParser.TypeContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.coreExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCoreExpression([NotNull] MParser.CoreExpressionContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.coreExpression"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCoreExpression([NotNull] MParser.CoreExpressionContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.creator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterCreator([NotNull] MParser.CreatorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.creator"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitCreator([NotNull] MParser.CreatorContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterIfStatement([NotNull] MParser.IfStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.ifStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitIfStatement([NotNull] MParser.IfStatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStatement([NotNull] MParser.StatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.statement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStatement([NotNull] MParser.StatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.functionCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctionCall([NotNull] MParser.FunctionCallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.functionCall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctionCall([NotNull] MParser.FunctionCallContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.arguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArguments([NotNull] MParser.ArgumentsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.arguments"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArguments([NotNull] MParser.ArgumentsContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterWhileStatement([NotNull] MParser.WhileStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.whileStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitWhileStatement([NotNull] MParser.WhileStatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterProgram([NotNull] MParser.ProgramContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.program"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitProgram([NotNull] MParser.ProgramContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.primitiveType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrimitiveType([NotNull] MParser.PrimitiveTypeContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.primitiveType"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrimitiveType([NotNull] MParser.PrimitiveTypeContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.breakStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBreakStatement([NotNull] MParser.BreakStatementContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.breakStatement"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBreakStatement([NotNull] MParser.BreakStatementContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.variableDecleration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariableDecleration([NotNull] MParser.VariableDeclerationContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.variableDecleration"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariableDecleration([NotNull] MParser.VariableDeclerationContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.parameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParameters([NotNull] MParser.ParametersContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.parameters"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParameters([NotNull] MParser.ParametersContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="MParser.variable"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVariable([NotNull] MParser.VariableContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="MParser.variable"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVariable([NotNull] MParser.VariableContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace Compiler.Parser.Antlr
