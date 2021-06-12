using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using LazenLang.Typechecking;

namespace LazenLang.Parsing.Ast.Types
{
    class NativeArrayType
    {
        public static ArrayType Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.L_BRACKET);

            TypeDesc type;
            try
            {
                type = parser.TryConsumer(TypeNode.Consume).Type;
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected type between brackets"),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.R_BRACKET, false);

            return new ArrayType(type);
        }
    }
}