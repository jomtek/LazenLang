using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    abstract class AtomType : Type
    {
        public static AtomType Consume(Parser parser)
        {
            AtomType atomType = null;

            switch (parser.LookAhead().Type)
            {
                case TokenInfo.TokenType.BOOL:
                    atomType = parser.TryConsumer(AtomBool.Consume);
                    break;
                case TokenInfo.TokenType.CHAR:
                    atomType = parser.TryConsumer(AtomChar.Consume);
                    break;
                case TokenInfo.TokenType.DOUBLE:
                    atomType = parser.TryConsumer(AtomDouble.Consume);
                    break;
                case TokenInfo.TokenType.INT:
                    atomType = parser.TryConsumer(AtomInt.Consume);
                    break;
                case TokenInfo.TokenType.STRING:
                    atomType = parser.TryConsumer(AtomString.Consume);
                    break;
                default:
                    break;
            }

            if (atomType == null)
                throw new ParserError(new FailedConsumer(), parser.Cursor);

            return atomType;
        }
    }
}
