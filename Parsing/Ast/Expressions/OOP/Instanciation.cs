using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Parsing.Display;
using System.Text;
using Parsing.Ast;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Expressions.OOP
{
    public class Instanciation : Expr, IPrettyPrintable
    {
        public Identifier ClassName;
        public TypeDescNode[] Generics;
        public ExprNode[] Arguments;

        public Instanciation(Identifier className, TypeDescNode[] generics, ExprNode[] arguments)
        {
            ClassName = className;
            Generics = generics;
            Arguments = arguments;
        }

        public static Instanciation Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NEW);

            Identifier className;
            TypeDescNode[] generics;
            ExprNode[] arguments;

            try
            {
                className = parser.TryConsumer(Identifier.Consume);
            } 
            catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.IDENTIFIER),
                    parser.Cursor
                );
            }

            generics = Utils.ParseGenerics(parser);

            parser.Eat(TokenInfo.TokenType.L_PAREN, false);
            arguments = Utils.ParseSequence(parser, ExprNode.Consume);
            parser.Eat(TokenInfo.TokenType.R_PAREN, false);

            return new Instanciation(className, generics, arguments);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("Instanciation");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Class Name: {ClassName.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Generic Arguments: {Display.Utils.PrettyArray(Generics, level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Arguments: {Display.Utils.PrettyArray(Arguments, level + 1)}");

            return sb.ToString();
        }
    }
}
