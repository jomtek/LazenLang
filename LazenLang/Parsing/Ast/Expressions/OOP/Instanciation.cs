using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Types;

namespace LazenLang.Parsing.Ast.Expressions.OOP
{
    class Instanciation : Expr
    {
        public Identifier ClassName;
        public TypeNode[] GenericArgs;
        public ExprNode[] Arguments;

        public Instanciation(Identifier className,TypeNode[] genericArgs, ExprNode[] arguments)
        {
            ClassName = className;
            GenericArgs = genericArgs;
            Arguments = arguments;
        }

        public static Instanciation Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NEW);

            Identifier className;
            TypeNode[] genericArgs;
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

            genericArgs = Utils.ParseGenericArgs(parser);

            parser.Eat(TokenInfo.TokenType.L_PAREN, false);
            arguments = Utils.ParseSequence(parser, ExprNode.Consume);
            parser.Eat(TokenInfo.TokenType.R_PAREN, false);

            return new Instanciation(className, genericArgs, arguments);
        }

        public override string Pretty()
        {
            string prettyGenericArgs = "";
            for (int i = 0; i < GenericArgs.Length; i++)
            {
                TypeNode node = GenericArgs[i];
                prettyGenericArgs += node.Pretty();
                if (i != GenericArgs.Length - 1) prettyGenericArgs += ", ";
            }

            return $"Instanciation(classname: {ClassName.Pretty()}, genericArgs: {{{prettyGenericArgs}}}, arguments: {Utils.PrettyArgs(Arguments)}";
        }
    }
}
