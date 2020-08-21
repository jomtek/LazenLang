using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Expressions
{
    class NullExpr : Expr
    {
        public static NullExpr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NULL);
            return new NullExpr();
        }

        public override string Pretty()
        {
            return "NullExpr()";
        }
    }
}
