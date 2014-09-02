lexer grammar MLexer;

// Keywords
Break : 'break';
Class : 'class';
Void : 'void';
Int : 'int';
This : 'this';
If : 'if';
Else : 'else';
For : 'for';
Static : 'static';
Return : 'return';
New : 'new';
Double : 'double';
String : 'string';

// Brackets
LeftParen : '(' ;
RightParen : ')';
LeftBrace : '{';
RightBrace : '}';
LeftBracket : '[';
RightBracket : ']';

// Operators
Less : '<';
LessEqual : '<=';
Greater : '>';
GreaterEqual : '>=';
Equal : '==';
NotEqual : '!=';

Plus : '+';
Minus : '-';
Star : '*';
StarStar : '**';
Div : '/';
Mod : '%';

AndAnd : '&&';
OrOr : '||';
Not : '!';

Semi : ';';
Colon : ':';
Comma : ',';

Assign : '=';

Dot : '.';

Identifier : Letter LetterAndDigit*;

IntegerConstant
    : '-'? [0-9]+;

StringLiteral
    :   '"' StringCharacters? '"'
    ;

Whitespace
    :   [ \t\n]+ -> skip
    ;

Comment
    :   '/*' .*? '*/' -> skip
    ;

LineComment
    :   '//' ~[\r\n]* -> skip
    ;


fragment Nondigit : [a-zA-Z_];
fragment Digit : [0-9] ;
fragment Letter : [a-zA-Z_];
fragment LetterAndDigit : [a-zA-Z0-9_];


fragment
StringCharacters
    :   StringCharacter+
    ;

fragment
StringCharacter
    :   ~["\\]
    ;