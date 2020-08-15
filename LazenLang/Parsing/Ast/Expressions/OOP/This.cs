using System;
using LazenLang.Lexing;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions.OOP
{
    class This : Expr
    {
        public static This Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.THIS);
            return new This();
        }

        public override string Pretty()
        {
            return "This()";
        }
    }
}