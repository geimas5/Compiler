parser grammar MParser;

options {
  tokenVocab=MLexer;
}

@header { 
	using Compiler.SyntaxTree; 
}


program returns [ProgramNode programNode]
@init{$programNode = nodeFactory.CreateProgramNode(_localctx);}
    : (functionDecleration {$programNode.Functions.Add($functionDecleration.func);})+ EOF;

variableDecleration returns [VariableDecleration decleration]
    : variable Assign expression Semi { $decleration = nodeFactory.CreateInitializedVariableDecleration(_localctx, $variable.var, $expression.expr); }
    | variable Semi  { $decleration = nodeFactory.CreateUnInitializedVariableDecleration(_localctx, $variable.var); }
    ;

functionDecleration returns [FunctionDecleration func]
    : Void Identifier LeftParen parameters RightParen statementBlock { $func = nodeFactory.CreateVoidFunctionDecleration(_localctx, $Identifier.text, $parameters.vars, $statementBlock.stmts); }
	| Void Identifier LeftParen RightParen statementBlock { $func = nodeFactory.CreateVoidFunctionDecleration(_localctx, $Identifier.text, new List<VariableNode>(), $statementBlock.stmts); }
    | type Identifier LeftParen parameters RightParen statementBlock { $func = nodeFactory.CreateReturningFunctionDecleration(_localctx, $Identifier.text, $parameters.vars, $statementBlock.stmts, $type.typeNode); }
	| type Identifier LeftParen RightParen statementBlock { $func = nodeFactory.CreateReturningFunctionDecleration(_localctx, $Identifier.text, new List<VariableNode>(), $statementBlock.stmts, $type.typeNode); }
    ;

parameters returns [List<VariableNode> vars]
@init{ $vars = new List<VariableNode>(); }
   : first=variable (Comma variable { $vars.Add($variable.var); } )* { $vars.Add($first.var); }
   ;

variable returns [VariableNode var]
   : type Identifier { $var = nodeFactory.CreateVariableNode(_localctx, $type.typeNode, nodeFactory.CreateVariableIdNode(_localctx, $Identifier.text)); }
   ;

type returns [TypeNode typeNode]
   : primitiveType { $typeNode = nodeFactory.CreateTypeNode(_localctx, $primitiveType.primitive); }
   | t=type LeftBracket RightBracket  { $typeNode = nodeFactory.CreateTypeNodeArrayOfType(_localctx, $t.typeNode.Type); }
   ;

primitiveType returns [PrimitiveType primitive]
   : Int { $primitive = PrimitiveType.Int; }
   | String { $primitive = PrimitiveType.String; }
   | Double { $primitive = PrimitiveType.Double; }
   | Bool { $primitive = PrimitiveType.Boolean; }
   ;

statementOrBlock returns [List<StatementNode> stmts]
   : statementBlock { $stmts = $statementBlock.stmts; }
   | statement { $stmts = $statement.stmts; }
   ;

statementBlock returns [List<StatementNode> stmts]
@init{ $stmts = new List<StatementNode>(); }
   : LeftBrace (statement { $stmts.AddRange($statement.stmts); })* RightBrace
   ;

statement returns [List<StatementNode> stmts]
@init{ $stmts = new List<StatementNode>(); }
   : expression Semi {$stmts.Add(nodeFactory.CreateExpressionStatement(_localctx, $expression.expr)); }
   | ifStatement { $stmts.Add($ifStatement.stmt); }
   | whileStatement { $stmts.Add($whileStatement.stmt); }
   | forStatement { $stmts.Add($forStatement.stmt); }
   | returnStatement { $stmts.Add($returnStatement.stmt); }
   | breakStatement { $stmts.Add($breakStatement.stmt); }
   | variableDecleration { $stmts.Add($variableDecleration.decleration); }
   ;

ifStatement returns [IfStatement stmt]
   : If LeftParen expression RightParen body=statementOrBlock Else els=statementOrBlock { $stmt = nodeFactory.CreateIfStatement(_localctx, $expression.expr, $body.stmts, $els.stmts); }
   | If LeftParen expression RightParen body=statementOrBlock { $stmt = nodeFactory.CreateIfStatement(_localctx, $expression.expr, $body.stmts); }
   ;

whileStatement returns [WhileStatement stmt]
   : While LeftParen expression RightParen statementOrBlock { $stmt = nodeFactory.CreateWhileStatement(_localctx, $expression.expr, $statementOrBlock.stmts); }
   ;

forStatement returns [ForStatement stmt]
   : For LeftParen init=expression Comma cond=expression Comma after=expression? RightParen statementOrBlock 
       { $stmt = nodeFactory.CreateForStatement(_localctx, $init.expr, $cond.expr, $after.expr, $statementOrBlock.stmts); }
   ;

returnStatement returns [ReturnStatement stmt]
   : Return expression Semi { $stmt = nodeFactory.CreateReturnStatement(_localctx, $expression.expr); }
   | Return Semi { $stmt = nodeFactory.CreateReturnStatement(_localctx); }
   ;

breakStatement returns [BreakStatement stmt]
   : Break Semi { $stmt = nodeFactory.CreateBreakStatement(_localctx); }
   ;

expression returns [ExpressionNode expr]
@init { BinaryOperator op = BinaryOperator.Multiply; }
    : coreExpression { $expr =  $coreExpression.expr; }
   | name=expression LeftBracket indx=expression RightBracket { $expr = nodeFactory.CreateIndexerExpression(_localctx, $name.expr, $indx.expr); }
   | functionCall { $expr = $functionCall.expr; }
   | creator { $expr = $creator.expr; }
   | left=expression StarStar right=expression { $expr = nodeFactory.CreateBinaryOperatorExpression(_localctx, $left.expr, $right.expr, BinaryOperator.Exponensiation); }
   | left=expression (Star { op = BinaryOperator.Multiply; } | Div { op = BinaryOperator.Divide; } | Mod { op = BinaryOperator.Mod; }) right=expression { $expr = nodeFactory.CreateBinaryOperatorExpression(_localctx, $left.expr, $right.expr, op); }
   | left=expression (Plus  { op = BinaryOperator.Add; }  | Minus  { op = BinaryOperator.Subtract; } ) right=expression { $expr = nodeFactory.CreateBinaryOperatorExpression(_localctx, $left.expr, $right.expr, op); }
   | Minus right=expression { $expr = nodeFactory.CreateUnaryExpression(_localctx, UnaryOperator.Negation, $right.expr); }
   | left=expression (Less { op = BinaryOperator.Less; } | LessEqual { op = BinaryOperator.LessEqual; } | Greater { op = BinaryOperator.Greater; } | GreaterEqual { op = BinaryOperator.GreaterEqual; } ) right=expression { $expr = nodeFactory.CreateBinaryOperatorExpression(_localctx, $left.expr, $right.expr, op); }
   | left=expression (NotEqual { op = BinaryOperator.NotEqual; } | Equal { op = BinaryOperator.Equal; } ) right=expression { $expr = nodeFactory.CreateBinaryOperatorExpression(_localctx, $left.expr, $right.expr, op); }
   | left=expression AndAnd right=expression  { $expr = nodeFactory.CreateBinaryOperatorExpression(_localctx, $left.expr, $right.expr, BinaryOperator.And); }
   | left=expression OrOr right=expression { $expr = nodeFactory.CreateBinaryOperatorExpression(_localctx, $left.expr, $right.expr, BinaryOperator.Or); }
   | Not right=expression  { $expr = nodeFactory.CreateUnaryExpression(_localctx, UnaryOperator.Not, $right.expr); }
   | <assoc=right>left=expression Assign right=expression { $expr = nodeFactory.CreateAssignmentExpression(_localctx, $left.expr, $right.expr); }
   ;

coreExpression returns [ExpressionNode expr]
    : LeftParen expression RightParen { $expr = $expression.expr; }
    | constant { $expr = nodeFactory.CreateConstantExpression(_localctx, $constant.const); }
    | Identifier { $expr = nodeFactory.CreateVariableExpression(_localctx, nodeFactory.CreateVariableIdNode(_localctx, $Identifier.text));  }
    ;

creator returns [CreatorExpression expr]
	: New primitiveType creatorSizes { $expr = nodeFactory.CreateArrayCreatorExpression(_localctx, $primitiveType.primitive, $creatorSizes.sizes ); }
    ;

creatorSizes returns [List<ExpressionNode> sizes]
@init { $sizes = new List<ExpressionNode>(); }
	:  LeftBracket first=expression RightBracket (LeftBracket expression RightBracket { $sizes.Add($expression.expr); })* { $sizes.Add($first.expr); }
	;

functionCall returns [FunctionCallExpression expr]
    : Identifier LeftParen arguments RightParen { $expr = nodeFactory.CreateFunctionCallExpression(_localctx, $Identifier.text, $arguments.args ); }
	| Identifier LeftParen RightParen { $expr = nodeFactory.CreateFunctionCallExpression(_localctx, $Identifier.text, new List<ExpressionNode>() ); }
    ;

arguments returns [List<ExpressionNode> args]
@init{ $args = new List<ExpressionNode>(); }
    : first=expression { $args.Add($first.expr); } (Comma expression { $args.Add($expression.expr); } )*
    ;

constant returns [ConstantNode const]
   : IntegerConstant { $const = nodeFactory.CreateIntegerConstant(_localctx, $IntegerConstant.text); }
   | StringConstant { $const = nodeFactory.CreateStringConstant(_localctx, $StringConstant.text); }
   | BooleanConstant { $const = nodeFactory.CreateBooleanConstant(_localctx, $BooleanConstant.text); }
   ;