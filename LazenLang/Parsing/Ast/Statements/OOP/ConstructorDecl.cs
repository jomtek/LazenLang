using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Lexing;
using System.Linq;
using LazenLang.Parsing.Display;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.OOP
{
    public class ConstructorDecl : Instr, IPrettyPrintable
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
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block for constructor declaration"),
                    parser.Cursor
                );
            }

            return new ConstructorDecl(domain, block);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("ConstructorDecl");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Domain: {Display.Utils.PrettyArray(Domain, level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"{Block.Pretty(level + 1)}"); 

            return sb.ToString();
        }
    }
}