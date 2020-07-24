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

        public ExprNode Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.DOUBLE_LIT).Value;

            return new ExprNode(
                new DoubleLit(Convert.ToDouble(literal)),
                parser.cursor
            );
        }
    }
}