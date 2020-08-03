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
            } catch (FormatException ex)
            {
                throw new ParserError(
                    new InvalidDoubleLit(literal),
                    parser.cursor
                );
            }
        }

        public new string ToString()
        {
            return $"DoubleLit({Value})"; 
        }
    }
}