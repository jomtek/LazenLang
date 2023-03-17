using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System.Linq;
using System.Runtime.InteropServices;

namespace LazenLang.Parsing.Ast
{
    /*public class TypevarSeq
    {
        public Identifier[] Sequence;
        public TypevarSeq(Identifier[] typevars)
        {
            Sequence = typevars;
        }

        public static TypevarSeq Consume(Parser parser)
        {
            Identifier[] typevars = new Identifier[0];

            bool lessToken = true;
            try
            {
                parser.Eat(TokenInfo.TokenType.LESS);
            }
            catch (ParserError)
            {
                lessToken = false;
            }

            if (lessToken)
            {
                typevars = Utils.ParseSequence(parser, Identifier.Consume);
                if (typevars.Count() == 0)
                {
                    throw new ParserError(
                        new ExpectedElementException("Expected one or more generic types"),
                        parser.Cursor
                    );
                }

                try
                {
                    parser.Eat(TokenInfo.TokenType.GREATER);
                }
                catch (ParserError)
                {
                    throw new ParserError(
                        new ExpectedTokenException(TokenInfo.TokenType.GREATER),
                        parser.Cursor
                    );
                }
            }

            return new TypevarSeq(typevars);
        }
    }*/
}