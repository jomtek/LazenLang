using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using Parsing.Ast;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Types
{
    public class FuncType : TypeDesc, IPrettyPrintable
    {
        public TypeDesc[] Domain;
        public TypeDesc Codomain;

        public FuncType(TypeDesc[] domain, TypeDesc codomain)
        {
            Domain = domain;
            Codomain = codomain;
        }

        public static FuncType Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FUNC_TYPE);
            parser.Eat(TokenInfo.TokenType.L_PAREN, false);

            TypeDesc[] domain;
            TypeDesc codomain;

            domain = Utils.ParseSequence(parser, (Parser p) => TypeDescNode.Consume(p).Value);
            if (domain.Length == 0)
            {
                throw new ParserError(
                    new InvalidElementException("Invalid domain for function type"),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.R_PAREN, false);
            parser.Eat(TokenInfo.TokenType.COLON, false);

            try
            {
                codomain = parser.TryConsumer(TypeDescNode.Consume).Value;
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected type after COLON token"),
                    parser.Cursor
                );
            }

            return new FuncType(domain, codomain);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("FuncType");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Domain: {Display.Utils.PrettyArray(Domain, level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Codomain: {Codomain.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}