parser grammar MParser;

options {
  tokenVocab=MLexer;
}

program : (classDecleration | interfaceDecleration)+ EOF;
            
classDecleration
     : Class Identifier Colon Identifier LeftBrace member* RightBrace
     | Class Identifier LeftBrace member* RightBrace
     ;

interfaceDecleration
     : Interface Identifier Colon Identifier LeftBrace prototype* RightBrace
     | Interface Identifier LeftBrace prototype* RightBrace
     ;

member
    : methodDecleration
    | variableDecleration;

prototype
    : Void Identifier LeftParen parameters? RightParen Semi
    | type Identifier LeftParen parameters? RightParen Semi
    ;

variableDecleration
    : type Identifier Assign expression Semi
    | type Identifier Semi
    ;

methodDecleration
    : Static? Void Identifier LeftParen parameters? RightParen statementBlock
    | Static? type Identifier LeftParen parameters? RightParen statementBlock
    ;

parameters
   : variable (Comma variable)*
   ;

variable
   : type Identifier
   ;

type
   : Int
   | String
   | Double
   | type LeftBracket RightBracket
   | Identifier
   ;

statementBlock
   : LeftBrace statement* RightBrace
   ;

statement
   : expression Semi
   | ifStatement
   | whileStatement
   | forStatement
   | returnStatement
   | breakStatement
   | variableDecleration
   | statementBlock
   ;

ifStatement
   : If LeftParen expression RightParen statement Else statement
   | If LeftParen expression RightParen statement
   ;

whileStatement
   : While LeftParen expression RightParen statement
   ;

forStatement
   : For LeftParen expression Comma expression Comma expression? RightParen
   ;

returnStatement
   : Return expression Semi
   | Return Semi
   ;

breakStatement
   : Break Semi
   ;

expression
    : coreExpression
   | expression LeftBracket expression RightBracket
   | expression Dot expression
   | methodCall
   | creator
   | expression StarStar expression
   | expression (Star | Div | Mod) expression
   | expression (Plus | Minus) expression
   | Minus expression
   |  expression (Less | LessEqual | Greater | GreaterEqual) expression
   |  expression (NotEqual | Equal ) expression
   | expression AndAnd expression
   | expression OrOr expression
   | Not expression
   | <assoc=right>expression Assign expression
   ;

coreExpression
    : LeftParen expression RightParen
    | This
    | constant
    | Identifier
    ;

creator
    : New type LeftBracket expression RightBracket
    | New type LeftParen RightParen
    ;

methodCall
    : Identifier LeftParen arguments? RightParen
    ;

arguments
    : expression (Comma expression)*
    ;

constant
   : IntegerConstant
   | StringLiteral
   ;