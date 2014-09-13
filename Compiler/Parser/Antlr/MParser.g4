parser grammar MParser;

options {
  tokenVocab=MLexer;
}

@header { using Compiler.SyntaxTree; }


program returns [ProgramNode programNode]
@init{$programNode = new Compiler.SyntaxTree.ProgramNode(Utility.CreateLocation(_localctx));}
    : (functionDecleration {$programNode.Functions.Add($functionDecleration.func);})+ EOF;

variableDecleration returns [VariableDecleration decleration]
    : variable Assign expression Semi { $decleration = new InitializedVariableDecleration(Utility.CreateLocation(_localctx), $variable.var, $expression.expr); }
    | variable Semi  { $decleration = new UnInitializedVariableDecleration(Utility.CreateLocation(_localctx), $variable.var); }
    ;

functionDecleration returns [FunctionDecleration func]
    : Void Identifier LeftParen parameters RightParen statementBlock { $func = new VoidFunctionDecleration(Utility.CreateLocation(_localctx), $Identifier.text, $parameters.vars, $statementBlock.stmts); }
	| Void Identifier LeftParen RightParen statementBlock { $func = new VoidFunctionDecleration(Utility.CreateLocation(_localctx), $Identifier.text, new List<VariableNode>(), $statementBlock.stmts); }
    | type Identifier LeftParen parameters RightParen statementBlock { $func = new ReturningFunctionDecleration(Utility.CreateLocation(_localctx), $Identifier.text, $parameters.vars, $statementBlock.stmts, $type.typeNode); }
	| type Identifier LeftParen RightParen statementBlock { $func = new ReturningFunctionDecleration(Utility.CreateLocation(_localctx), $Identifier.text, new List<VariableNode>(), $statementBlock.stmts, $type.typeNode); }
    ;

parameters returns [List<VariableNode> vars]
@init{ $vars = new List<VariableNode>(); }
   : first=variable (Comma variable { $vars.Add($variable.var); } )* { $vars.Add($first.var); }
   ;

variable returns [VariableNode var]
   : type Identifier { $var = new VariableNode(Utility.CreateLocation(_localctx), $type.typeNode, new VariableIdNode(Utility.CreateLocation(_localctx), $Identifier.text)); }
   ;

type returns [TypeNode typeNode]
   : Int { $typeNode = new PrimitiveType(Utility.CreateLocation(_localctx), Types.Int); }
   | String { $typeNode = new PrimitiveType(Utility.CreateLocation(_localctx), Types.String); }
   | Double { $typeNode = new PrimitiveType(Utility.CreateLocation(_localctx), Types.Double); }
   | t=type LeftBracket RightBracket  { $typeNode = new ArrayType(Utility.CreateLocation(_localctx), $t.typeNode); }
   ;

statementBlock returns [List<StatementNode> stmts]
@init{ $stmts = new List<StatementNode>(); }
   : LeftBrace (statement { $stmts.AddRange($statement.stmts); })* RightBrace
   ;

statement returns [List<StatementNode> stmts]
@init{ $stmts = new List<StatementNode>(); }
   : expression Semi {$stmts.Add(new ExpressionStatement(Utility.CreateLocation(_localctx), $expression.expr)); }
   | ifStatement { $stmts.Add($ifStatement.stmt); }
   | whileStatement { $stmts.Add($whileStatement.stmt); }
   | forStatement { $stmts.Add($forStatement.stmt); }
   | returnStatement { $stmts.Add($returnStatement.stmt); }
   | breakStatement { $stmts.Add($breakStatement.stmt); }
   | variableDecleration { $stmts.Add($variableDecleration.decleration); }
   | statementBlock { $stmts.AddRange($statementBlock.stmts); }
   ;

ifStatement returns [IfStatement stmt]
   : If LeftParen expression RightParen body=statement Else els=statement { $stmt = new IfStatement(Utility.CreateLocation(_localctx), $expression.expr, $body.stmts, $els.stmts); }
   | If LeftParen expression RightParen body=statement { $stmt = new IfStatement(Utility.CreateLocation(_localctx), $expression.expr, $body.stmts); }
   ;

whileStatement returns [WhileStatement stmt]
   : While LeftParen expression RightParen statement { $stmt = new WhileStatement(Utility.CreateLocation(_localctx), $expression.expr, $statement.stmts); }
   ;

forStatement returns [ForStatement stmt]
   : For LeftParen init=expression Comma cond=expression Comma after=expression? RightParen statement { $stmt = new ForStatement(Utility.CreateLocation(_localctx), $init.expr, $cond.expr, $after.expr, $statement.stmts); }
   ;

returnStatement returns [ReturnStatement stmt]
   : Return expression Semi { $stmt = new ReturnExpressionStatement(Utility.CreateLocation(_localctx), $expression.expr); }
   | Return Semi { $stmt = new VoidReturnStatement(Utility.CreateLocation(_localctx)); }
   ;

breakStatement returns [BreakStatement stmt]
   : Break Semi { $stmt = new BreakStatement(Utility.CreateLocation(_localctx)); }
   ;

expression returns [ExpressionNode expr]
@init { BinaryOperator op = BinaryOperator.Multiply; }
    : coreExpression { $expr =  $coreExpression.expr; }
   | name=expression LeftBracket indx=expression RightBracket { $expr = new IndexerExpression(Utility.CreateLocation(_localctx), $name.expr, $indx.expr); }
   //| expression Dot expression
   | functionCall { $expr = $functionCall.expr; }
   | creator { $expr = $creator.expr; }
   | left=expression StarStar right=expression { $expr = new BinaryOperatorExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr, BinaryOperator.Exponensiation); }
   | left=expression (Star { op = BinaryOperator.Multiply; } | Div { op = BinaryOperator.Divide; } | Mod { op = BinaryOperator.Mod; }) right=expression { $expr = new BinaryOperatorExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr, op); }
   | left=expression (Plus  { op = BinaryOperator.Add; }  | Minus  { op = BinaryOperator.Subtract; } ) right=expression { $expr = new BinaryOperatorExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr, op); }
   | Minus expression { $expr = new UnaryExpression(Utility.CreateLocation(_localctx), UnaryOperator.Negation, $expression.expr); }
   | left=expression (Less { op = BinaryOperator.Less; } | LessEqual { op = BinaryOperator.LessEqual; } | Greater { op = BinaryOperator.Greater; } | GreaterEqual { op = BinaryOperator.GreaterEqual; } ) right=expression { $expr = new BinaryOperatorExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr, op); }
   | left=expression (NotEqual { op = BinaryOperator.NotEqual; } | Equal { op = BinaryOperator.Equal; } ) right=expression { $expr = new BinaryOperatorExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr, op); }
   | left=expression AndAnd right=expression  { $expr = new BinaryOperatorExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr, BinaryOperator.And); }
   | left=expression OrOr right=expression { $expr = new BinaryOperatorExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr, BinaryOperator.Or); }
   | Not expression  { $expr = new UnaryExpression(Utility.CreateLocation(_localctx), UnaryOperator.Not, $expression.expr); }
   | <assoc=right>left=expression Assign right=expression { $expr = new AssignmentExpression(Utility.CreateLocation(_localctx), $left.expr, $right.expr); }
   ;

coreExpression returns [ExpressionNode expr]
    : LeftParen expression RightParen { $expr = $expression.expr; }
    //| This
    | constant { $expr = new ConstantExpression(Utility.CreateLocation(_localctx), $constant.const); }
    | Identifier { $expr = new VariableExpression(Utility.CreateLocation(_localctx), new VariableIdNode(Utility.CreateLocation(_localctx), $Identifier.text));  }
    ;

creator returns [CreatorExpression expr]
    //: New type LeftParen RightParen
	: New type LeftBracket expression RightBracket { $expr = new ArrayCreatorExpression(Utility.CreateLocation(_localctx), $type.typeNode, $expression.expr ); }
    ;

functionCall returns [FunctionCallExpression expr]
    : Identifier LeftParen arguments RightParen { $expr = new FunctionCallExpression(Utility.CreateLocation(_localctx), $Identifier.text, $arguments.args ); }
	| Identifier LeftParen RightParen { $expr = new FunctionCallExpression(Utility.CreateLocation(_localctx), $Identifier.text, new List<ExpressionNode>() ); }
    ;

arguments returns [List<ExpressionNode> args]
@init{ $args = new List<ExpressionNode>(); }
    : first=expression { $args.Add($first.expr); } (Comma expression { $args.Add($expression.expr); } )*
    ;

constant returns [ConstantNode const]
   : IntegerConstant { $const = new IntegerConstant(Utility.CreateLocation(_localctx), int.Parse($IntegerConstant.text)); }
   | StringConstant { $const = new StringConstant(Utility.CreateLocation(_localctx), $StringConstant.text); }
   ;