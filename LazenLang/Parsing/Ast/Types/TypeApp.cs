using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types
{
    class TypeApp : Type
    {
        public NameType BaseType;
        public TypeNode[] GenericArgs;
        
        public TypeApp(NameType baseType, TypeNode[] genericArgs)
        {
            BaseType = baseType;
            GenericArgs = genericArgs;
        }

        public static TypeApp Consume(Parser parser)
        {
            NameType baseType;
            TypeNode[] genericArgs;

            baseType = parser.TryConsumer(NameType.Consume);
            parser.Eat(TokenInfo.TokenType.LESS);
            genericArgs = Utils.ParseSequence(parser, TypeNode.Consume);

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

        public override string Pretty()
        {

            string result = "";
            for (int i = 0; i < GenericArgs.Count(); i++)
            {
                TypeNode arg = GenericArgs[i];
                result += arg.Type.Pretty();
                if (i != GenericArgs.Count()) result += ", ";
            }
            return $"TypeApp(base: {BaseType.Pretty()}, args: {{{result}}}";
        }
    }
}
