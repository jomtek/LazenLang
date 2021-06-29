using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.OOP
{
    public class ClassDecl : Instr, IPrettyPrintable
    {
        public bool PublicAccess { get; }
        public bool Abstract { get; }
        public Identifier Name;
        public TypevarSeq Typevars;
        public Block Block;

        public ClassDecl(bool publicAccess, bool abstract_, Identifier name, TypevarSeq typevars, Block block)
        {
            PublicAccess = publicAccess;
            Abstract = abstract_;
            Name = name;
            Typevars = typevars;
            Block = block;
        }

        public static ClassDecl Consume(Parser parser)
        {
            bool publicAccess = Utils.ParseAccessModifier(parser);
            bool abstract_ = true;

            try
            {
                parser.Eat(TokenInfo.TokenType.ABSTRACT);
            }
            catch (ParserError)
            {
                abstract_ = false;
            }

            parser.Eat(TokenInfo.TokenType.CLASS);

            Identifier name;
            TypevarSeq typevars;
            Block block;

            try
            {
                name = parser.TryConsumer(Identifier.Consume);
            }
            catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected identifier after CLASS token"),
                    parser.Cursor
                );
            }

            typevars = parser.TryConsumer(TypevarSeq.Consume);

            try
            {
                block = parser.TryConsumer((Parser p) => Block.Consume(p, true, false, true));
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block for class declaration"),
                    parser.Cursor
                );
            }

            return new ClassDecl(publicAccess, abstract_, name, typevars, block);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("ClassDecl");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"PublicAccess: {PublicAccess}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"IsAbstract: {Abstract}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Typevars: {Display.Utils.PrettyArray(Typevars.Sequence, level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"{Block.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}
