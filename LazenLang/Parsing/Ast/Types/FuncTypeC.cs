using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Typechecking;

namespace LazenLang.Parsing.Ast.Types
{
    class FuncTypeC
    {
        public static FuncType Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FUNC_TYPE);
            parser.Eat(TokenInfo.TokenType.L_PAREN, false);

            TypeDesc[] domain;
            TypeDesc codomain;

            domain = Utils.ParseSequence(parser, (Parser p) => TypeNode.Consume(p).Type);
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
                codomain = parser.TryConsumer(TypeNode.Consume).Type;
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
    }
}