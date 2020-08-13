using System.Linq;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    class Signature
    {
        public Identifier Name { get; }
        public TypevarSeq Typevars { get; }
        public Param[] Domain { get; }
        public TypeNode Codomain { get; }

        public Signature(Identifier name, TypevarSeq typevars, Param[] domain, TypeNode codomain)
        {
            Name = name;
            Typevars = typevars;
            Domain = domain;
            Codomain = codomain;
        }

        public static Signature Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FUNC);

            Identifier name = null;
            TypevarSeq typevars = null;
            Param[] domain = new Param[0];
            TypeNode codomain = null;

            // func foo(baz: String, bax) -> Int
            // func foo(baz)
            // func foo()

            name = parser.TryConsumer(Identifier.Consume);
            typevars = parser.TryConsumer(TypevarSeq.Consume);

            parser.Eat(TokenInfo.TokenType.L_PAREN, false);
            domain = Utils.ParseSequence(parser, Param.Consume);
            parser.Eat(TokenInfo.TokenType.R_PAREN, false);

            bool arrow = true;
            Token arrowToken = null;
            try
            {
                arrowToken = parser.Eat(TokenInfo.TokenType.ARROW);
            }
            catch (ParserError)
            {
                arrow = false;
            }

            if (arrow)
            {
                try
                {
                    codomain = parser.TryConsumer(TypeNode.Consume);
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new ExpectedElementException("Expected type after ARROW token"),
                        parser.Cursor
                    );
                }
            }

            return new Signature(name, typevars, domain, codomain);
        }

        public string Pretty()
        {
            string prettyDomain = "";
            string prettyCodomain = "none";

            for (int i = 0; i < Domain.Count(); i++)
            {
                Param param = Domain[i];
                prettyDomain += param.Pretty();
                if (i != Domain.Count() - 1) prettyDomain += ", ";
            }

            if (Codomain != null)
                prettyCodomain = Codomain.Pretty();

            return $"Signature(name: {Name.Pretty()}, typevars: {Typevars.Pretty()}, domain: {{{prettyDomain}}}, codomain: {prettyCodomain})";
        }
    }
}