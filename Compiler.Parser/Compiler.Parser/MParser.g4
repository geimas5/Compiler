parser grammar MParser;

program : (classDecleration | interfaceDecleration)+ EOF;
            
classDecleration
     : 'class' Identifier ':' Identifier '{' member* '}'
     | 'class' Identifier '{' member* '}'
     ;

interfaceDecleration
     : 'interface' Identifier ':' Identifier '{' prototype* '}'
     | 'interface' Identifier '{' prototype* '}'
     ;

member
    : methodDecleration
    | variableDecleration;

prototype
    : 'void' Identifier '(' parameters? ')' ';'
    | type Identifier '(' parameters? ')' ';'
    ;

variableDecleration
    : type Identifier '=' expression ';'
    | type Identifier ';'
    ;

methodDecleration
    : 'static'? 'void' Identifier '(' parameters? ')' statementBlock
    | 'static'? type Identifier '(' parameters? ')' statementBlock
    ;

parameters
   : variable (',' variable)*
   ;

variable
   : type Identifier
   ;

type
   : 'int'
   | 'string'
   | 'double'
   | type '[' ']'
   | Identifier
   ;

statementBlock
   : '{' statement* '}'
   ;

statement
   : expression ';'
   | ifStatement
   | whileStatement
   | forStatement
   | returnStatement
   | breakStatement
   | variableDecleration
   | statementBlock
   ;

ifStatement
   : 'if' '(' expression ')' statement 'else' statement
   | 'if' '(' expression ')' statement
   ;

whileStatement
   : 'while' '(' expression ')' statement
   ;

forStatement
   : 'for' '(' expression ',' expression ',' expression? ')'
   ;

returnStatement
   : 'return' expression ';'
   | 'return' ';'
   ;

breakStatement
   : 'break' ';'
   ;

expression
    : coreExpression
   | expression '[' expression ']'
   | expression '.' expression
   | methodCall
   | creator
   | expression '**' expression
   | expression ('*' | '/' | '%') expression
   | expression ('+' | '-') expression
   | '-' expression
   |  expression ('<' | '<=' | '>' | '>=') expression
   |  expression ('!=' | '==' ) expression
   | expression '&&' expression
   | expression '||' expression
   | '!' expression
   | <assoc=right>expression '=' expression
   ;

coreExpression
    : '(' expression ')'
    | 'this'
    | constant
    | Identifier
    ;

creator
    : 'new' type '[' expression ']'
    | 'new' type '(' ')'
    ;

methodCall
    : Identifier '(' arguments? ')'
    ;

arguments
    : expression (',' expression)*
    ;

constant
   : IntegerConstant
   | StringLiteral
   ;