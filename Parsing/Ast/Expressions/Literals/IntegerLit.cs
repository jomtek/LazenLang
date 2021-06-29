using System;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    public class IntegerLit : Literal, IPrettyPrintable
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

        public override string Pretty(int level)
        {
            return $"IntegerLit: {Value}";
        }
    }
}