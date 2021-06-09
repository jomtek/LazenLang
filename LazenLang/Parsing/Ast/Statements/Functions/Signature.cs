using System.Linq;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    public class Signature : IPrettyPrintable
    {
        public bool PublicAccess { get; }
        public bool Static { get; }
        public Identifier Name { get; }
        public TypevarSeq Typevars { get; }
        public Param[] Domain { get; }
        public TypeNode Codomain { get; }

        public Signature(bool publicAccess, bool static_, Identifier name, TypevarSeq typevars, Param[] domain, TypeNode codomain)
        {
            PublicAccess = publicAccess;
            Static = static_;
            Name = name;
            Typevars = typevars;
            Domain = domain;
            Codomain = codomain;
        }

        public static Signature Consume(Parser parser, bool inClass = false)
        {
            bool publicAccess = Utils.ParseAccessModifier(parser);
            bool static_ = false;

            if (inClass)
                static_ = Utils.ParseStatic(parser);

            parser.Eat(TokenInfo.TokenType.FUNC);

            Identifier name = null;
            TypevarSeq typevars = null;
            Param[] domain = new Param[0];
            TypeNode codomain = null;

            // func foo(baz: String, bax) -> Int
            // func foo(baz)
            // func foo()

            name = parser.TryConsumer(Identifier.Consume);
            typevars = parser.TryConsumer(TypevarSeq.Consume);

            parser.Eat(TokenInfo.TokenType.L_PAREN, false);
            domain = Utils.ParseSequence(parser, Param.Consume);

            parser.Eat(TokenInfo.TokenType.R_PAREN, false);

            bool arrow = true;
            Token arrowToken = null;
            try
            {
                arrowToken = parser.Eat(TokenInfo.TokenType.ARROW);
            }
            catch (ParserError)
            {
                arrow = false;
            }

            if (arrow)
            {
                try
                {
                    codomain = parser.TryConsumer(TypeNode.Consume);
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new ExpectedElementException("Expected type after ARROW token"),
                        parser.Cursor
                    );
                }
            }

            return new Signature(publicAccess, static_, name, typevars, domain, codomain);
        }

        public string Pretty(int level)
        {
            var sb = new StringBuilder("Signature");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Static: {Static}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Public Access: {PublicAccess}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Typevars: {Display.Utils.PrettyArray(Typevars.Sequence, level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Domain: {Display.Utils.PrettyArray(Domain, level + 1)}");
            if (Codomain != null) sb.AppendLine(Display.Utils.Indent(level + 1) + $"Codomain: {Codomain.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}