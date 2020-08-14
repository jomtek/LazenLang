using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Expressions.OOP
{
    class Instanciation : Expr
    {
        public Identifier ClassName;
        public ExprNode[] Arguments;

        public Instanciation(Identifier className, ExprNode[] arguments)
        {
            ClassName = className;
            Arguments = arguments;
        }

        public static Instanciation Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NEW);

            Identifier className;
            ExprNode[] arguments;

            try
            {
                className = parser.TryConsumer(Identifier.Consume);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.IDENTIFIER),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.L_PAREN, false);
            arguments = Utils.ParseSequence(parser, ExprNode.Consume);
            parser.Eat(TokenInfo.TokenType.R_PAREN, false);

            return new Instanciation(className, arguments);
        }

        public override string Pretty()
        {
            return $"Instanciation(classname: {ClassName.Pretty()}, arguments: {Utils.PrettyArgs(Arguments)}";
        }
    }
}
