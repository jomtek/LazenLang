using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Lexing;
using System;
using System.Linq;

namespace LazenLang.Parsing.Ast.Expressions
{
    class Lambda : Expr
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
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected expression after ARROW token"),
                    parser.Cursor
                );
            }
         
            return new Lambda(domain, returnValue);
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
            return $"Lambda(domain: {{{prettyDomain}}}, returnValue: {ReturnValue.Pretty()})";
        }
    }
}
