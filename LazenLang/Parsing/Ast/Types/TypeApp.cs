using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Typechecking;

namespace LazenLang.Parsing.Ast.Types
{
    class TypeAppC
    {
        public static TypeApp Consume(Parser parser)
        {
            NameType baseType;
            TypeDesc[] genericArgs;

            baseType = parser.TryConsumer(NameTypeC.Consume);
            parser.Eat(TokenInfo.TokenType.LESS);
            genericArgs = Utils.ParseSequence(parser, (Parser p) => TypeNode.Consume(p).Type);

            try
            {
                parser.Eat(TokenInfo.TokenType.GREATER);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected GREATER token after type sequence"),
                    parser.Cursor
                );
            }

            if (genericArgs.Length == 0)
            {
                throw new ParserError(
                    new InvalidElementException("Invalid type sequence for type application"),
                    parser.Cursor
                );
            }

            return new TypeApp(baseType, genericArgs);
        }
    }
}
