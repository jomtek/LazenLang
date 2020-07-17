using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Literals
{
    abstract class Literal : Expr
    {}

    class StringLit : Literal
    {
        public string Value;
        
        public StringLit(string value)
        {
            Value = value;
        }

        public StringLit consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.STRING_LIT).Value;
            return new StringLit(literal);
        }
    }
}
