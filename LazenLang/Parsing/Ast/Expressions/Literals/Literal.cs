using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    abstract class Literal : Expr
    {
        public static ExprNode Consume(Parser parser)
        {
            var literal = parser.TryManyConsumers(
                new Func<Parser, Literal>[] {
                    BooleanLit.Consume,
                    CharLit.Consume,
                    DoubleLit.Consume,
                    Identifier.Consume,
                    IntegerLit.Consume,
                    StringLit.Consume
                }
            );

            return new ExprNode(
                literal, parser.Cursor
            );
        }
    }
}