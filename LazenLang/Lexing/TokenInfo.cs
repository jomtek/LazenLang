namespace LazenLang.Lexing
{
    class TokenInfo
    {
        public enum TokenType
        {
            L_PAREN, R_PAREN, L_BRACKET, R_BRACKET, L_CURLY_BRACKET, R_CURLY_BRACKET,
            COMMA, DOT, APOSTROPHE, QUOTE, EOL, IDENTIFIER, COLON, ARROW, BIG_ARROW,
            INTEGER_LIT, DOUBLE_LIT, STRING_LIT, CHAR_LIT, BOOLEAN_LIT, NULL,

            // Keywords    
            VAR, CONST, THIS, IN, FOR, RETURN, IF, ELSE, ELIF, WHILE, BREAK, CONTINUE, FUNC,
            FUNC_TYPE, NEW, CLASS, STATIC, ABSTRACT, CONSTRUCTOR, PRIVATE, TRY, CATCH, THROW,
            INTERFACE,

            // Atom types
            BOOL, CHAR, DOUBLE, INT, STRING, VOID,

            // Operators
            ASSIGN, EQ, NOT_EQ, BOOLEAN_AND, BOOLEAN_OR, GREATER, LESS, PLUS, MINUS,
            DIVIDE, MULTIPLY, POWER, MODULO, NEG, GREATER_EQ, LESS_EQ, PLUS_EQ, MINUS_EQ,
            DIVIDE_EQ, MULTIPLY_EQ, POWER_EQ, MODULO_EQ, DOUBLE_DOT,

            // Other
            SINGLE_LINE_COMMENT,
            MULTI_LINE_COMMENT,
            SPACE, TAB,
            OTHER
        }

        public static (string, TokenType)[] IdenRegexTable = new (string, TokenType)[]
        {
            (@"^(true|false)",                        TokenType.BOOLEAN_LIT),
            (@"^var(?![a-zA-Z_0-9])",                 TokenType.VAR),
            (@"^const(?![a-zA-Z_0-9])",               TokenType.CONST),
            (@"^for(?![a-zA-Z_0-9])",                 TokenType.FOR),
            (@"^return(?![a-zA-Z_0-9])",              TokenType.RETURN),
            (@"^if(?![a-zA-Z_0-9])",                  TokenType.IF),
            (@"^else(?![a-zA-Z_0-9])",                TokenType.ELSE),
            (@"^elif(?![a-zA-Z_0-9])",                TokenType.ELIF),
            (@"^while(?![a-zA-Z_0-9])",               TokenType.WHILE),
            (@"^break(?![a-zA-Z_0-9])",               TokenType.BREAK),
            (@"^continue(?![a-zA-Z_0-9])",            TokenType.CONTINUE),
            (@"^try(?![a-zA-Z_0-9])",                 TokenType.TRY),
            (@"^catch(?![a-zA-Z_0-9])",               TokenType.CATCH),
            (@"^this(?![a-zA-Z_0-9])",                TokenType.THIS),
            (@"^in(?![a-zA-Z_0-9])",                  TokenType.IN),
            (@"^throw(?![a-zA-Z_0-9])",               TokenType.THROW),
            (@"^func(?![a-zA-Z_0-9])",                TokenType.FUNC),
            (@"^Func(?![a-zA-Z_0-9])",                TokenType.FUNC_TYPE),
            (@"^class(?![a-zA-Z_0-9])",               TokenType.CLASS),
            (@"^new(?![a-zA-Z_0-9])",                 TokenType.NEW),
            (@"^private(?![a-zA-Z_0-9])",             TokenType.PRIVATE),
            (@"^interface(?![a-zA-Z_0-9])",           TokenType.INTERFACE),
            (@"^static(?![a-zA-Z_0-9])",              TokenType.STATIC),
            (@"^abstract(?![a-zA-Z_0-9])",            TokenType.ABSTRACT),
            (@"^constructor(?![a-zA-Z_0-9])",         TokenType.CONSTRUCTOR),
            (@"^Bool(?![a-zA-Z_0-9])",                TokenType.BOOL),
            (@"^Char(?![a-zA-Z_0-9])",                TokenType.CHAR),
            (@"^Double(?![a-zA-Z_0-9])",              TokenType.DOUBLE),
            (@"^Int(?![a-zA-Z_0-9])",                 TokenType.INT),
            (@"^String(?![a-zA-Z_0-9])",              TokenType.STRING),
            (@"^Void(?![a-zA-Z_0-9])",                TokenType.VOID),
            (@"^null(?![a-zA-Z_0-9])",                TokenType.NULL),
            (@"^(?![0-9])[0-9_a-zA-Z\u00C0-\u017F]+", TokenType.IDENTIFIER),
        };

        public static (string, TokenType)[] OtherRegexTable = new (string, TokenType)[]
        {
            (@"^\t",                                  TokenType.TAB),
            (@"^\n",                                  TokenType.EOL),
            (@"^ ",                                   TokenType.SPACE),
            (@"^\(",                                  TokenType.L_PAREN),
            (@"^\)",                                  TokenType.R_PAREN),
            (@"^\,",                                  TokenType.COMMA),
            (@"^\.",                                  TokenType.DOT),
            (@"^[0-9]+",                              TokenType.INTEGER_LIT),
            (@"^\""[^\""\\]*(\\.[^\""\\]*)*\""",      TokenType.STRING_LIT),
            (@"^\'[^\'\\]*(\\.[^\'\\]*)*\'",          TokenType.CHAR_LIT),
            (@"^[0-9]+\.[0-9]+",                      TokenType.DOUBLE_LIT),
            (@"^\[",                                  TokenType.L_BRACKET),
            (@"^\]",                                  TokenType.R_BRACKET),
            (@"^\{",                                  TokenType.L_CURLY_BRACKET),
            (@"^\}",                                  TokenType.R_CURLY_BRACKET),
            (@"^\/\/(.*)",                            TokenType.SINGLE_LINE_COMMENT),
            (@"^\/\*((((?!\/\*)(?!\*\/).)|\n)*)\*\/", TokenType.MULTI_LINE_COMMENT),
            (@"^\:",                                  TokenType.COLON),
            (@"^\>",                                  TokenType.GREATER),
            (@"^\<",                                  TokenType.LESS),
            (@"^\/",                                  TokenType.DIVIDE),
            (@"^\*",                                  TokenType.MULTIPLY),
            (@"^\=",                                  TokenType.ASSIGN),
            (@"^\+",                                  TokenType.PLUS),
            (@"^\-",                                  TokenType.MINUS),
            (@"^\%",                                  TokenType.MODULO),
            (@"^\^",                                  TokenType.POWER),
            (@"^\-\>",                                TokenType.ARROW),
            (@"^\=\>",                                TokenType.BIG_ARROW),
            (@"^\+\=",                                TokenType.PLUS_EQ),
            (@"^\-\=",                                TokenType.MINUS_EQ),
            (@"^\/\=",                                TokenType.DIVIDE_EQ),
            (@"^\*\=",                                TokenType.MULTIPLY_EQ),
            (@"^\^\=",                                TokenType.POWER_EQ),
            (@"^\%\=",                                TokenType.MODULO_EQ),
            (@"^\|\|",                                TokenType.BOOLEAN_OR),
            (@"^\&\&",                                TokenType.BOOLEAN_AND),
            (@"^\>\=",                                TokenType.GREATER_EQ),
            (@"^\<\=",                                TokenType.LESS_EQ),
            (@"^\=\=",                                TokenType.EQ),
            (@"^\!\=",                                TokenType.NOT_EQ),
            (@"^\!",                                  TokenType.NEG),
            (@"^\.\.",                                TokenType.DOUBLE_DOT),
            (@"^\'",                                  TokenType.APOSTROPHE),
            (@"^\""",                                 TokenType.QUOTE),
            (@"^.",                                   TokenType.OTHER),
        };
    }

}