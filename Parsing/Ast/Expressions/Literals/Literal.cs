using System;
using LazenLang.Lexing;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    public abstract class Literal : Expr
    {
        public static Literal Consume(Parser parser)
        {
            Literal expr = null;
            switch (parser.LookAhead().Type)
            {
                case TokenInfo.TokenType.BOOLEAN_LIT:
                    expr = parser.TryConsumer(BooleanLit.Consume);
                    break;
                case TokenInfo.TokenType.CHAR_LIT:
                    expr = parser.TryConsumer(CharLit.Consume);
                    break;
                case TokenInfo.TokenType.DOUBLE_LIT:
                    expr = parser.TryConsumer(DoubleLit.Consume);
                    break;
                case TokenInfo.TokenType.IDENTIFIER:
                    expr = parser.TryConsumer(Identifier.Consume);
                    break;
                case TokenInfo.TokenType.INTEGER_LIT:
                    expr = parser.TryConsumer(IntegerLit.Consume);
                    break;
                case TokenInfo.TokenType.STRING_LIT:
                    expr = parser.TryConsumer(StringLit.Consume);
                    break;
            }

            if (expr == null)
                throw new ParserError(new FailedConsumer(), parser.Cursor);

            return expr;
        }
    }
}