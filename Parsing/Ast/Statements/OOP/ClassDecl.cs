using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Display;
using Parsing.Ast;
using Parsing.Errors;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.OOP
{
    public class ClassDecl : Instr, IPrettyPrintable, ICreatesSingleBlock
    {
        public Identifier Name;
        public Block Block { get; set; }

        public ClassDecl(Identifier name, Block block)
        {
            Name = name;
            Block = block;
        }

        public static ClassDecl Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.CLASS);

            Identifier name;
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

            return new ClassDecl( name, block);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("ClassDecl");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"{Block.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}
