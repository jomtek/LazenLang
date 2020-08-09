using System.Linq;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    class Signature
    {
        public Identifier Name { get; }
        public Identifier[] GenericTypes { get; }
        public Param[] Domain { get; }
        public TypeNode Codomain { get; }

        public Signature(Identifier name, Identifier[] genericTypes, Param[] domain, TypeNode codomain)
        {
            Name = name;
            GenericTypes = genericTypes;
            Domain = domain;
            Codomain = codomain;
        }

        public static Signature Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FUNC);

            Identifier name = null;
            Identifier[] genericTypes = null;
            Param[] domain = null;
            TypeNode codomain = null;

            // func foo(baz: String, bax) -> Int
            // func foo(baz)
            // func foo()

            name = parser.TryConsumer(Identifier.Consume);

            bool lessToken = true;
            try
            {
                parser.Eat(TokenInfo.TokenType.LESS);
            } catch (ParserError)
            {
                lessToken = false;
            }

            if (lessToken)
            {
                genericTypes = Utils.ParseSequence(parser, Identifier.Consume);
                if (genericTypes.Count() == 0)
                {
                    throw new ParserError(
                        new ExpectedElementException("Expected one or more generic types"),
                        parser.Cursor
                    );
                }

                try
                {
                    parser.Eat(TokenInfo.TokenType.GREATER);
                } catch (ParserError)
                {
                    throw new ParserError(
                        new ExpectedTokenException(TokenInfo.TokenType.GREATER),
                        parser.Cursor
                    );
                }
            }

            try
            {
                parser.Eat(TokenInfo.TokenType.L_PAREN);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.L_PAREN),
                    parser.Cursor
                );
            }

            domain = Utils.ParseSequence(parser, Param.Consume);

            try
            {
                parser.Eat(TokenInfo.TokenType.R_PAREN);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.R_PAREN),
                    parser.Cursor
                );
            }

            bool arrow = true;
            try
            {
                parser.Eat(TokenInfo.TokenType.ARROW);
            } catch (ParserError)
            {
                arrow = false;
            }

            if (arrow)
            {
                try
                {
                    codomain = parser.TryConsumer(TypeNode.Consume);
                } catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new ExpectedElementException("Expected type after ARROW token"),
                        parser.Cursor
                    );
                }
            }

            return new Signature(name, genericTypes, domain, codomain);
        }

        public string Pretty()
        {
            string prettyGenericTypes = "";
            string prettyDomain = "";
            string prettyCodomain = "none";

            if (GenericTypes != null)
            {
                for (int i = 0; i < GenericTypes.Count(); i++)
                {
                    Identifier genericType = GenericTypes[i];
                    prettyGenericTypes += genericType.Value;
                    if (i != GenericTypes.Count() - 1) prettyGenericTypes += ", ";
                }
            }

            for (int i = 0; i < Domain.Count(); i++)
            {
                Param param = Domain[i];
                prettyDomain += param.Pretty();
                if (i != Domain.Count() - 1) prettyDomain += ", ";
            }

            if (Codomain != null) prettyCodomain = Codomain.Pretty();

            return $"Signature(name: {Name.Pretty()}, genericTypes: [{prettyGenericTypes}], domain: {{{prettyDomain}}}, codomain: {prettyCodomain})";
        }
    }
}