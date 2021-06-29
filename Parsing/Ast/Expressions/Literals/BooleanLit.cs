using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    public class BooleanLit : Literal, IPrettyPrintable
    {
        public bool Value;

        public BooleanLit(bool value)
        {
            Value = value;
        }

        public new static BooleanLit Consume(Parser parser)
        {
            string value = parser.Eat(TokenInfo.TokenType.BOOLEAN_LIT).Value;
            return new BooleanLit(Convert.ToBoolean(value));
        }

        public override string Pretty(int level)
        {
            string literal = Value ? "true" : "false";
            return $"BooleanLit: {literal}";
        }
    }
}