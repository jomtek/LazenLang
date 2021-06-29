using LazenLang.Lexing;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    public class StringLit : Literal, IPrettyPrintable
    {
        public string Value;
        
        public StringLit(string value)
        {
            Value = value;
        }

        public new static StringLit Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.STRING_LIT).Value;
            return new StringLit(literal);
        }

        public override string Pretty(int level)
        {
            return $"StringLit: \"{Value}\"";
        }
    }
}