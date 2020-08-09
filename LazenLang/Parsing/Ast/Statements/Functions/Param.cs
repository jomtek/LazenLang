using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    class Param
    {
        public Identifier Name;
        public TypeNode Type;

        public Param(Identifier name, TypeNode type)
        {
            Name = name;
            Type = type;
        }

        public static Param Consume(Parser parser)
        {
            Identifier name = null;
            TypeNode type = null;

            name = parser.TryConsumer(Identifier.Consume);

            bool colon = true;
            try
            {
                parser.Eat(TokenInfo.TokenType.COLON);
            } catch (ParserError)
            {
                colon = false;
            }

            if (colon)
            {
                try
                {
                    type = parser.TryConsumer(TypeNode.Consume);
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new ExpectedElementException("Expected type after COLON token"),
                        parser.Cursor
                    );
                }
            }
            return new Param(name, type);
        }

        public string Pretty()
        {
            string prettyType = "none";
            if (Type != null) prettyType = Type.Pretty();
            return $"Param({Name.Pretty()}: {prettyType})";
        }
    }
}
