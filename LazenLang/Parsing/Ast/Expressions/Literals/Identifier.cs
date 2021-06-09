using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using System;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    public class Identifier : Literal, IPrettyPrintable
    {
        public string Value;

        public Identifier(string value)
        {
            Value = value;
        }

        public new static Identifier Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.IDENTIFIER).Value;
            return new Identifier(literal);
        }

        public override bool Equals(object obj)
        {
            return Value == ((Identifier)obj).Value;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value);
        }

        public override string Pretty(int level)
        {
            return $"Identifier: {Value}";
        }
    }
}
