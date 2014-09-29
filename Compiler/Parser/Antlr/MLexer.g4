lexer grammar MLexer;

// Keywords
Break : 'break';
Void : 'void';
Int : 'int';
If : 'if';
Else : 'else';
For : 'for';
Return : 'return';
New : 'new';
Double : 'double';
String : 'string';
While : 'while';
Bool : 'bool';
BooleanConstant
    : 'true' 
	| 'false';


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
Comma : ',';

Assign : '=';

Identifier : Letter LetterAndDigit*;

IntegerConstant
    : Digit+;

DoubleConstant
    : Digit+ '.' Digit+
	; 

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