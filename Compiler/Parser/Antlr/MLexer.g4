lexer grammar MLexer;

// Keywords
Break : 'break';
Void : 'void';
Int : 'int';
If : 'if';
Else : 'else';
For : 'for';
Static : 'static'; 
Return : 'return';
New : 'new';
Double : 'double';
String : 'string';
While : 'while';

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

StringConstant
    :   '"' StringCharacters? '"'
    ;

Whitespace
    :   [ \t\n\r]+ -> skip
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