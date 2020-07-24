using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Expressions.Literals
{
    class Identifier : Literal
    {
        public string Value;

        public Identifier(string value)
        {
            Value = value;
        }

        public ExprNode Consume(Parser parser)
        {
            string literal = parser.Eat(TokenInfo.TokenType.IDENTIFIER).Value;
            return new ExprNode(
                new Identifier(literal),
                parser.cursor
            );
        }
    }
}
