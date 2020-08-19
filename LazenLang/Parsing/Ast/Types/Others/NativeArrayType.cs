using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types
{
    class NativeArrayType : Type
    {
        public TypeNode Type;

        public NativeArrayType(TypeNode type)
        {
            Type = type;
        }

        public static NativeArrayType Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.L_BRACKET);

            TypeNode type;
            try
            {
                type = parser.TryConsumer(TypeNode.Consume);
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected type between brackets"),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.R_BRACKET, false);

            return new NativeArrayType(type);
        }

        public override string Pretty()
        {
            return $"NativeArrayType(type: {Type.Pretty()})";
        }
    }
}