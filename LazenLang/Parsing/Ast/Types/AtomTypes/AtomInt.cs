using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    class AtomInt : AtomType
    {
        public new static AtomInt Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.INT);
            return new AtomInt();
        }

        public override string Pretty()
        {
            return "AtomInt()";
        }
    }
}