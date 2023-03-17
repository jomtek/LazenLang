namespace LazenLang.Lexing
{
    public static class TokenInfo
    {
        public enum TokenType
        {
            // (     )        [          ]          [                ]
            L_PAREN, R_PAREN, L_BRACKET, R_BRACKET, L_CURLY_BRACKET, R_CURLY_BRACKET,
            // ,   .    '           "      abc         :      ; 
            COMMA, DOT, APOSTROPHE, QUOTE, IDENTIFIER, COLON, SEMI_COLON,
            // 123       12.3        "abc"       "a"       true
            INTEGER_LIT, DOUBLE_LIT, STRING_LIT, CHAR_LIT, BOOLEAN_LIT,

            // Keywords    
            IN, FOR, RETURN, IF, ELSE, WHILE, BREAK, CONTINUE, FUNC, CLASS,

            // Operators
            // =    ==  !=      &&           ||          >        <
            ASSIGN, EQ, NOT_EQ, BOOLEAN_AND, BOOLEAN_OR, GREATER, LESS,
            // +  -      /       *         ^      %       !    >=          <=
            PLUS, MINUS, DIVIDE, MULTIPLY, POWER, MODULO, NOT, GREATER_EQ, LESS_EQ,
            
            // ..
            DOUBLE_DOT,
            
            // +=      -=        /=         *=           ^=        %=        
            //PLUS_EQ, MINUS_EQ, DIVIDE_EQ, MULTIPLY_EQ, POWER_EQ, MODULO_EQ,

            // Other
            EOL, IGNORE, OTHER
        }

        public static (string, TokenType)[] IdenRegexTable = new (string, TokenType)[]
        {
            (@"^(true|false)",                        TokenType.BOOLEAN_LIT),
            (@"^for(?![a-zA-Z_0-9])",                 TokenType.FOR),
            (@"^return(?![a-zA-Z_0-9])",              TokenType.RETURN),
            (@"^if(?![a-zA-Z_0-9])",                  TokenType.IF),
            (@"^else(?![a-zA-Z_0-9])",                TokenType.ELSE),
            (@"^while(?![a-zA-Z_0-9])",               TokenType.WHILE),
            (@"^break(?![a-zA-Z_0-9])",               TokenType.BREAK),
            (@"^continue(?![a-zA-Z_0-9])",            TokenType.CONTINUE),
            (@"^in(?![a-zA-Z_0-9])",                  TokenType.IN),
            (@"^func(?![a-zA-Z_0-9])",                TokenType.FUNC),
            (@"^class(?![a-zA-Z_0-9])",               TokenType.CLASS),
            (@"^(?![0-9])[0-9_a-zA-Z\u00C0-\u017F]+", TokenType.IDENTIFIER),
        };

        public static (string, TokenType)[] OtherRegexTable = new (string, TokenType)[]
        {
            (@"^\n",                                  TokenType.EOL),
            (@"^\t+",                                 TokenType.IGNORE),
            (@"^\s+",                                 TokenType.IGNORE),
            (@"^\/\/(.*)",                            TokenType.IGNORE),
            (@"^\/\*((((?!\/\*)(?!\*\/).)|\n)*)\*\/", TokenType.IGNORE),
            (@"^\;",                                  TokenType.SEMI_COLON),
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
            (@"^\:",                                  TokenType.COLON),
            (@"^\%",                                  TokenType.MODULO),
            (@"^\^",                                  TokenType.POWER),
            /*(@"^\+\=",                                TokenType.PLUS_EQ),
            (@"^\-\=",                                TokenType.MINUS_EQ),
            (@"^\/\=",                                TokenType.DIVIDE_EQ),
            (@"^\*\=",                                TokenType.MULTIPLY_EQ),
            (@"^\^\=",                                TokenType.POWER_EQ),
            (@"^\%\=",                                TokenType.MODULO_EQ),*/
            (@"^\+",                                  TokenType.PLUS),
            (@"^\-",                                  TokenType.MINUS),
            (@"^\|\|",                                TokenType.BOOLEAN_OR),
            (@"^\&\&",                                TokenType.BOOLEAN_AND),
            (@"^\>\=",                                TokenType.GREATER_EQ),
            (@"^\<\=",                                TokenType.LESS_EQ),
            (@"^\=\=",                                TokenType.EQ),
            (@"^\!\=",                                TokenType.NOT_EQ),
            (@"^\>",                                  TokenType.GREATER),
            (@"^\<",                                  TokenType.LESS),
            (@"^\/",                                  TokenType.DIVIDE),
            (@"^\*",                                  TokenType.MULTIPLY),
            (@"^\=",                                  TokenType.ASSIGN),
            (@"^\!",                                  TokenType.NOT),
            (@"^\.\.",                                TokenType.DOUBLE_DOT),
            (@"^\'",                                  TokenType.APOSTROPHE),
            (@"^\""",                                 TokenType.QUOTE),
            (@"^.",                                   TokenType.OTHER),
        };
    }

}