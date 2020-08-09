using LazenLang.Lexing;
using System.Collections.Generic;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    class NegNum
    {
        public Expr Value;

        public NegNum(Expr value)
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
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive())
                    throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected expression after PLUS or MINUS prefix"),
                    parser.Cursor
                );
            }

            if (prefix.Type == TokenInfo.TokenType.MINUS)
                return new NegExpr(expression);
            else
                return expression;
        }
    }
}
