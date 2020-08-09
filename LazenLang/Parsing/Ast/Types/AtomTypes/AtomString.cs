using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    class AtomString : AtomType
    {
        public new static AtomString Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.STRING);
            return new AtomString();
        }

        public override string Pretty()
        {
            return "AtomString()";
        }
    }
}