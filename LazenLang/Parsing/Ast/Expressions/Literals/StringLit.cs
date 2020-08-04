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

        public new static StringLit Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.STRING_LIT).Value;
            return new StringLit(literal);
        }

        public override string Pretty()
        {
            return $"StringLit(\"{Value}\")";
        }
    }
}