using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    class AtomBool : AtomType
    {
        public new static AtomBool Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.BOOL);
            return new AtomBool();
        }

        public override string Pretty()
        {
            return "AtomBool()";
        }
    }
}