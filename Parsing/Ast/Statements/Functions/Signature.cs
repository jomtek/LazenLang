using System.Linq;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using System.Text;
using Parsing.Ast;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    public class Signature : Instr, IPrettyPrintable
    {
        public Identifier Name { get; }
        public Param[] Domain { get; }
        public TypeDescNode Codomain { get; }

        public Signature(Identifier name, Param[] domain, TypeDescNode codomain)
        {
            Name = name;
            Domain = domain;
            Codomain = codomain;
        }

        public static Signature Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FUNC);

            Identifier name = null;
            Param[] domain = new Param[0];
            TypeDescNode codomain = null;

            // func foo(baz: String, bax: Int): Int
            // func foo()

            name = parser.TryConsumer(Identifier.Consume);

            parser.Eat(TokenInfo.TokenType.L_PAREN, false);
            domain = Utils.ParseSequence(parser, Param.Consume);

            parser.Eat(TokenInfo.TokenType.R_PAREN, false);

            parser.Eat(TokenInfo.TokenType.COLON, false);

            try
            {
                codomain = parser.TryConsumer(TypeDescNode.Consume);
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected type after COLON token"),
                    parser.Cursor
                );
            }

            return new Signature(name, domain, codomain);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("Signature");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Domain: {Display.Utils.PrettyArray(Domain, level + 1)}");
            if (Codomain != null) sb.AppendLine(Display.Utils.Indent(level + 1) + $"Codomain: {Codomain.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}