using LazenLang.Lexing;
using LazenLang.Typechecking;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    abstract class AtomTypeC
    {
        // TODO
        /*
        public static AtomType Consume(Parser parser)
        {
            AtomType atomType = null;

            switch (parser.LookAhead().Type)
            {
                case TokenInfo.TokenType.BOOL:
                    parser.Eat(TokenInfo.TokenType.BOOL);
                    atomType = new BoolType();
                    break;

                case TokenInfo.TokenType.CHAR:
                    parser.Eat(TokenInfo.TokenType.CHAR);
                    atomType = new CharType();
                    break;

                case TokenInfo.TokenType.DOUBLE:
                    parser.Eat(TokenInfo.TokenType.DOUBLE);
                    atomType = new DoubleType();
                    break;

                case TokenInfo.TokenType.INT:
                    parser.Eat(TokenInfo.TokenType.INT);
                    atomType = new IntType();
                    break;

                case TokenInfo.TokenType.STRING:
                    parser.Eat(TokenInfo.TokenType.STRING);
                    atomType = new StringType();
                    break;

                case TokenInfo.TokenType.VOID:
                    parser.Eat(TokenInfo.TokenType.VOID);
                    atomType = new VoidType();
                    break;

                default:
                    break;
            }

            if (atomType == null)
                throw new ParserError(new FailedConsumer(), parser.Cursor);

            return atomType;
        }
        */
    }
}
