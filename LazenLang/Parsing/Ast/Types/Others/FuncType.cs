using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types
{
    class FuncType : Type
    {
        public TypeNode[] Domain { get; }
        public TypeNode Codomain { get; }

        public FuncType(TypeNode[] domain, TypeNode codomain)
        {
            Domain = domain;
            Codomain = codomain;
        }

        public static FuncType Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FUNC_TYPE);
            parser.Eat(TokenInfo.TokenType.L_PAREN, false);

            TypeNode[] domain;
            TypeNode codomain;

            domain = Utils.ParseSequence(parser, TypeNode.Consume);
            if (domain.Length == 0)
            {
                throw new ParserError(
                    new InvalidElementException("Invalid domain for function type"),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.R_PAREN, false);
            parser.Eat(TokenInfo.TokenType.BIG_ARROW, false);

            try
            {
                codomain = parser.TryConsumer(TypeNode.Consume);
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected type after BIG ARROW token"),
                    parser.Cursor
                );
            }

            return new FuncType(domain, codomain);
        }

        public override string Pretty()
        {
            string prettyDomain = "";
            for (int i = 0; i < Domain.Length; i++)
            {
                TypeNode node = Domain[i];
                prettyDomain += node.Pretty();
                if (i != Domain.Length - 1) prettyDomain += ", ";
            }

            return $"FuncType(domain: {{{prettyDomain}}}, codomain: {Codomain.Pretty()})";
        }
    }
}