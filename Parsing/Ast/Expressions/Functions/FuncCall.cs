using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Parsing.Display;
using Parsing.Ast;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions
{
    public class FuncCall : Expr, IPrettyPrintable
    {
        public Identifier Name { get; }
        public ExprNode[] Arguments { get; }

        public FuncCall(Identifier name, ExprNode[] arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public static FuncCall Consume(Parser parser)
        {
            Identifier name = null;
            ExprNode[] arguments = new ExprNode[0];

            name = parser.TryConsumer(Identifier.Consume);

            parser.Eat(TokenInfo.TokenType.L_PAREN);

            arguments = Utils.ParseSequence(parser, ExprNode.Consume);

            try
            {
                parser.Eat(TokenInfo.TokenType.R_PAREN);
            }
            catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.R_PAREN),
                    parser.Cursor
                );
            }

            return new FuncCall(name, arguments);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("FuncCall");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Arguments: " + Display.Utils.PrettyArray(Arguments, level + 1));

            return sb.ToString();
        }
    }
}
