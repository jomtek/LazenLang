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
    public class TypeApp : TypeDesc, IPrettyPrintable
    {
        public NameType BaseType;
        public TypeDescNode[] Generics;

        public TypeApp(NameType baseType, TypeDescNode[] generics)
        {
            BaseType = baseType;
            Generics = generics;
        }

        public static TypeApp Consume(Parser parser)
        {
            NameType baseType;
            TypeDescNode[] genericArgs;

            baseType = parser.TryConsumer(NameType.Consume);
            parser.Eat(TokenInfo.TokenType.LESS);
            genericArgs = Utils.ParseSequence(parser, (Parser p) => TypeDescNode.Consume(p));

            try
            {
                parser.Eat(TokenInfo.TokenType.GREATER);
            }
            catch (ParserError)
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

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("TypeApp");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"BaseType: {BaseType.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Generics: {Display.Utils.PrettyArray(Generics, level + 1)}");

            return sb.ToString();
        }
    }
}
