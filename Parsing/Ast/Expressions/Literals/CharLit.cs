using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    public class CharLit : Literal, IPrettyPrintable
    {
        public char Value;

        public CharLit(char value)
        {
            Value = value;
        }

        public new static CharLit Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.CHAR_LIT).Value;

            if (literal.Length != 1)
            {
                throw new ParserError(new InvalidCharLit(literal), parser.Cursor);
            }

            return new CharLit(literal[0]);
        }

        public override string Pretty(int level)
        {
            return $"CharLit: \'{Value}\'";
        }
    }
}
