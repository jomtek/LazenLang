using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    class StringLit : Literal
    {
        public string Value;
        
        public StringLit(string value)
        {
            Value = value;
        }

        public static StringLit Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.STRING_LIT).Value;
            return new StringLit(literal);
        }

        public new string ToString()
        {
            return $"StringLit(\"{Value}\")";
        }
    }
}