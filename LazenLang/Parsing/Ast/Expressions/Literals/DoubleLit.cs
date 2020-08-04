using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    class DoubleLit : Literal
    {
        public double Value;

        public DoubleLit(double value)
        {
            Value = value;
        }

        public new static DoubleLit Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.DOUBLE_LIT).Value;
            try
            {
                return new DoubleLit(Double.Parse(literal, CultureInfo.InvariantCulture));
            } catch (FormatException)
            {
                throw new ParserError(
                    new InvalidDoubleLit(literal),
                    parser.Cursor
                );
            }
        }

        public override string Pretty()
        {
            return $"DoubleLit({Value})"; 
        }
    }
}