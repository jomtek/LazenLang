using LazenLang.Lexing;
using System;
using System.Collections.Generic;
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
            return new DoubleLit(Convert.ToDouble(literal));
        }

        public new string ToString()
        {
            return $"DoubleLit({Value})"; 
        }
    }
}