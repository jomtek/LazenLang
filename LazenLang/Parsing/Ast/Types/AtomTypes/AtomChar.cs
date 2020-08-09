using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    class AtomChar : AtomType
    {
        public new static AtomChar Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.CHAR);
            return new AtomChar();
        }

        public override string Pretty()
        {
            return "AtomChar()";
        }
    }
}