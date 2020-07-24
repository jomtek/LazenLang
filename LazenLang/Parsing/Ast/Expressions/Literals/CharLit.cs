using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    class CharLit : Literal
    {
        public char Value;

        public CharLit(char value)
        {
            Value = value;
        }

        public ExprNode Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.CHAR_LIT).Value;

            if (literal.Length != 1)
                throw new ParserError(new InvalidCharLit(literal), parser.cursor);

            return new ExprNode(
                new CharLit(literal[0]),
                parser.cursor
            );
        }
    }
}
