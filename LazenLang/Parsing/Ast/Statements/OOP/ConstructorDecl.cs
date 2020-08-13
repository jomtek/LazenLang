using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Lexing;
using System.Linq;

namespace LazenLang.Parsing.Ast.Statements.OOP
{
    class ConstructorDecl : Instr
    {
        public Param[] Domain;
        public Block Block;

        public ConstructorDecl(Param[] domain, Block block)
        {
            Domain = domain;
            Block = block;
        }

        public static ConstructorDecl Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.CONSTRUCTOR);

            Block block;
            Param[] domain;

            parser.Eat(TokenInfo.TokenType.L_PAREN, false);
            domain = Utils.ParseSequence(parser, Param.Consume);
            parser.Eat(TokenInfo.TokenType.R_PAREN, false);

            try
            {
                block = parser.TryConsumer((Parser p) => Block.Consume(p));
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block for constructor declaration"),
                    parser.Cursor
                );
            }

            return new ConstructorDecl(domain, block);
        }

        public override string Pretty()
        {
            string prettyDomain = "";

            for (int i = 0; i < Domain.Count(); i++)
            {
                Param param = Domain[i];
                prettyDomain += param.Pretty();
                if (i != Domain.Count() - 1) prettyDomain += ", ";
            }

            return $"ConstructorDecl(domain: {{{prettyDomain}}}, block: {Block.Pretty()})";
        }
    }
}
