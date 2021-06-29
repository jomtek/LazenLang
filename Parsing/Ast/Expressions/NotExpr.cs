using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions
{
    public class NotExpr : Expr, IPrettyPrintable
    {
        public Expr Value;

        public NotExpr(Expr value)
        {
            Value = value;
        }

        public static Expr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NOT);

            ExprNode expression;

            try
            {
                expression = parser.TryConsumer(ExprNode.Consume);
            }
            catch
            {
                throw new ParserError(
                    new ExpectedElementException("Expected expression after NOT token"),
                    parser.Cursor
                );
            }

            return new NotExpr(expression.Value);
        }

        public override string Pretty(int level)
        {
            return $"NotExpr: {Value.Pretty(level)}";
        }
    }
}
