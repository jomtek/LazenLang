using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using System.Collections.Generic;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    public class NegExpr : Expr, IPrettyPrintable
    {
        public Expr Value;

        public NegExpr(Expr value)
        {
            Value = value;
        }

        public static Expr Consume(Parser parser)
        {
            Token prefix = parser.TryManyEats(new TokenInfo.TokenType[] { TokenInfo.TokenType.PLUS, TokenInfo.TokenType.MINUS });
         
            Expr expression;
            try
            {
                expression = parser.TryConsumer(ExprNode.Consume).Value;
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive())
                    throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected expression after PLUS or MINUS prefix"),
                    parser.Cursor
                );
            }

            if (prefix.Type == TokenInfo.TokenType.MINUS)
            {
                return new NegExpr(expression);
            }
            else
            {
                return expression;
            }
        }

        public override string Pretty(int level)
        {
            return $"NegExpr: {Value.Pretty(level)}";
        }
    }
}
