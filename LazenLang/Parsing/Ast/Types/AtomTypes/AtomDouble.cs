using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    class AtomDouble : AtomType
    {
        public new static AtomDouble Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.DOUBLE);
            return new AtomDouble();
        }

        public override string Pretty()
        {
            return "AtomDouble()";
        }
    }
}