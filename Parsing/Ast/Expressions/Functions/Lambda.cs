using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Lexing;
using System;
using System.Linq;
using LazenLang.Parsing.Display;
using System.Text;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Expressions
{
    public class Lambda : Expr, IPrettyPrintable
    {
        public Param[] Domain;
        public ExprNode ReturnValue;

        public Lambda(Param[] domain, ExprNode returnValue)
        {
            Domain = domain;
            ReturnValue = returnValue;
        }

        public static Lambda Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FUNC);
            parser.Eat(TokenInfo.TokenType.L_PAREN);

            Param[] domain;
            ExprNode returnValue;

            domain = Utils.ParseSequence(parser, Param.Consume);
            parser.Eat(TokenInfo.TokenType.R_PAREN, false);
            parser.Eat(TokenInfo.TokenType.ARROW, false);

            try
            {
                returnValue = parser.TryConsumer(ExprNode.Consume);
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected expression after ARROW token"),
                    parser.Cursor
                );
            }
         
            return new Lambda(domain, returnValue);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("Lambda");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Domain: {Display.Utils.PrettyArray(Domain, level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Return Value: {ReturnValue.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}
