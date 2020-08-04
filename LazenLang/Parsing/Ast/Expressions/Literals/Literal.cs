using System;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    abstract class Literal : Expr
    {
        public static Expr Consume(Parser parser)
        {
            return parser.TryManyConsumers(
                new Func<Parser, Literal>[] {
                    BooleanLit.Consume,
                    CharLit.Consume,
                    DoubleLit.Consume,
                    Identifier.Consume,
                    IntegerLit.Consume,
                    StringLit.Consume
                }
            );
        }
    }
}