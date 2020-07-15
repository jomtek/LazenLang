using System.Collections.Generic;
using System.Text.Json;

namespace LazenLang.Lexer
{
    public class TokenInfo
    {
        public enum TokenType
        {
            L_PAREN, R_PAREN, L_BRACKET, R_BRACKET, L_CURLY_BRACKET, R_CURLY_BRACKET,
            COMMA, DOT, APOSTROPHE, QUOTE, EOL, IDENTIFIER, COLON, ARROW,
            INTEGER_LIT, DOUBLE_LIT, STRING_LIT, CHAR_LIT, BOOLEAN_LIT,

            // Keywords    
            VAR, CONST, IN, FOR, RETURN, IF, ELSE, ELIF, WHILE, BREAK, CONTINUE, FUNC,
            NEW, CLASS, CONSTRUCTOR, PUBLIC, PRIVATE, STEP, TRY, CATCH, THROW, INTERFACE,

            // Operators
            ASSIGN, EQ, NOT_EQ, BOOLEAN_AND, BOOLEAN_OR, GREATER, LESS, PLUS, MINUS,
            DIVIDE, MULTIPLY, POWER, MODULO, NEG, GREATER_EQ, LESS_EQ, PLUS_EQ, MINUS_EQ,
            DIVIDE_EQ, MULTIPLY_EQ, POWER_EQ, MODULO_EQ, DOUBLE_DOT,

            // Other
            SINGLE_LINE_COMMENT,
            MULTI_LINE_COMMENT,
            OTHER
        }

        public static List<(string, TokenType)> RegexTable = new List<(string, TokenType)>()
        {
            (@"^\/\/(.*)",                            TokenType.SINGLE_LINE_COMMENT),
            (@"^\/\*((((?!\/\*)(?!\*\/).)|\n)*)\*\/", TokenType.MULTI_LINE_COMMENT),
            (@"^\n",                                  TokenType.EOL),
            (@"^\(",                                  TokenType.L_PAREN),
            (@"^\)",                                  TokenType.R_PAREN),
            (@"^\[",                                  TokenType.L_BRACKET),
            (@"^\]",                                  TokenType.R_BRACKET),
            (@"^\{",                                  TokenType.L_CURLY_BRACKET),
            (@"^\}",                                  TokenType.R_CURLY_BRACKET),
            (@"^\:",                                  TokenType.COLON),
            (@"^\-\>",                                TokenType.ARROW),
            (@"^\+\=",                                TokenType.PLUS_EQ),
            (@"^\-\=",                                TokenType.MINUS_EQ),
            (@"^\/\=",                                TokenType.DIVIDE_EQ),
            (@"^\*\=",                                TokenType.MULTIPLY_EQ),
            (@"^\^\=",                                TokenType.POWER_EQ),
            (@"^\%\=",                                TokenType.MODULO_EQ),
            (@"^\,",                                  TokenType.COMMA),
            (@"^\|\|",                                TokenType.BOOLEAN_OR),
            (@"^\&\&",                                TokenType.BOOLEAN_AND),
            (@"^[0-9]+\.[0-9]+",                      TokenType.DOUBLE_LIT),
            (@"^[0-9]+",                              TokenType.INTEGER_LIT),
            (@"^\>\=",                                TokenType.GREATER_EQ),
            (@"^\<\=",                                TokenType.LESS_EQ),
            (@"^\=\=",                                TokenType.EQ),
            (@"^\!\=",                                TokenType.NOT_EQ),
            (@"^\!",                                  TokenType.NEG),
            (@"^\>",                                  TokenType.GREATER),
            (@"^\<",                                  TokenType.LESS),
            (@"^\/",                                  TokenType.DIVIDE),
            (@"^\*",                                  TokenType.MULTIPLY),
            (@"^\=",                                  TokenType.ASSIGN),
            (@"^\+",                                  TokenType.PLUS),
            (@"^\-",                                  TokenType.MINUS),
            (@"^\%",                                  TokenType.MODULO),
            (@"^\^",                                  TokenType.POWER),
            (@"^\.\.",                                TokenType.DOUBLE_DOT),
            (@"^\.",                                  TokenType.DOT),
            (@"^\""[^\""\\]*(\\.[^\""\\]*)*\""",      TokenType.STRING_LIT),
            (@"^\'[^\'\\]*(\\.[^\'\\]*)*\'",          TokenType.CHAR_LIT),
            (@"^(True|False)",                        TokenType.BOOLEAN_LIT),
            (@"^\'",                                  TokenType.APOSTROPHE),
            (@"^\""",                                 TokenType.QUOTE),
            (@"^try(?![a-zA-Z_0-9])",                 TokenType.TRY),
            (@"^catch(?![a-zA-Z_0-9])",               TokenType.CATCH),
            (@"^throw(?![a-zA-Z_0-9])",               TokenType.THROW),
            (@"^var(?![a-zA-Z_0-9])",                 TokenType.VAR),
            (@"^const(?![a-zA-Z_0-9])",               TokenType.CONST),
            (@"^in(?![a-zA-Z_0-9])",                  TokenType.IN),
            (@"^for(?![a-zA-Z_0-9])",                 TokenType.FOR),
            (@"^return(?![a-zA-Z_0-9])",              TokenType.RETURN),
            (@"^if(?![a-zA-Z_0-9])",                  TokenType.IF),
            (@"^else(?![a-zA-Z_0-9])",                TokenType.ELSE),
            (@"^elif(?![a-zA-Z_0-9])",                TokenType.ELIF),
            (@"^while(?![a-zA-Z_0-9])",               TokenType.WHILE),
            (@"^break(?![a-zA-Z_0-9])",               TokenType.BREAK),
            (@"^continue(?![a-zA-Z_0-9])",            TokenType.CONTINUE),
            (@"^func(?![a-zA-Z_0-9])",                TokenType.FUNC),
            (@"^class(?![a-zA-Z_0-9])",               TokenType.CLASS),
            (@"^new(?![a-zA-Z_0-9])",                 TokenType.NEW),
            (@"^step(?![a-zA-Z_0-9])",                TokenType.STEP),
            (@"^public(?![a-zA-Z_0-9])",              TokenType.PUBLIC),
            (@"^private(?![a-zA-Z_0-9])",             TokenType.PRIVATE),
            (@"^interface(?![a-zA-Z_0-9])",           TokenType.INTERFACE),
            (@"^(?![0-9])[0-9_a-zA-Z\u00C0-\u017F]+", TokenType.IDENTIFIER),
            (@"^.",                                   TokenType.OTHER)
        };
    }

}