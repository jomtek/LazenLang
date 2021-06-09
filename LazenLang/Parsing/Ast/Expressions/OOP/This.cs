using System;
using LazenLang.Lexing;
using System.Collections.Generic;
using System.Text;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Expressions.OOP
{
    public class This : Expr, IPrettyPrintable
    {
        public static This Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.THIS);
            return new This();
        }

        public override string Pretty(int level)
        {
            return "This";
        }
    }
}