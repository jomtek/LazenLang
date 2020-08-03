using System;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    class IntegerLit : Literal
    {
        public int Value;

        public IntegerLit(int value)
        {
            Value = value;
        }

        public new static IntegerLit Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.INTEGER_LIT).Value;
            return new IntegerLit(Convert.ToInt32(literal));
        }

        public new string ToString()
        {
            return $"IntegerLit({Value})";
        }
    }
}