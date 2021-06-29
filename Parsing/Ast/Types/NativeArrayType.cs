using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using Parsing.Ast;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Types
{
    public class NativeArrayType : TypeDesc, IPrettyPrintable
    {
        public TypeDesc Type;

        public NativeArrayType(TypeDesc type)
        {
            Type = type;
        }

        public static NativeArrayType Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.L_BRACKET);

            TypeDesc type;
            try
            {
                type = parser.TryConsumer(TypeDescNode.Consume).Value;
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected TYPE between brackets"),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.R_BRACKET, false);

            return new NativeArrayType(type);
        }

        public override string Pretty(int level)
        {
            return "NativeArrayType: " + Type.Pretty(level);
        }
    }
}