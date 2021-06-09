using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Expressions
{
    public class NullExpr : Expr, IPrettyPrintable
    {
        public static NullExpr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NULL);
            return new NullExpr();
        }

        public override string Pretty(int level)
        {
            return "Null";
        }
    }
}
