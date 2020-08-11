using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Types.AtomTypes;
using LazenLang.PrettyPrinter;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Types
{
    abstract class Type
    {
        public virtual string Pretty()
        {
            return "Type";
        }
    }

    class TypeNode
    {
        public Type Type { get;  }
        public CodePosition Position { get; }

        public TypeNode(Type type, CodePosition position)
        {
            Type = type;
            Position = position;
        }

        public static TypeNode Consume(Parser parser)
        {
            CodePosition oldCursor = parser.Cursor;
            Type type = null;
            
            switch (parser.LookAhead().Type)
            {
                case TokenInfo.TokenType.L_BRACKET:
                    type = parser.TryConsumer(NativeArrayType.Consume);
                    break;

                case TokenInfo.TokenType.IDENTIFIER:
                    type = parser.TryManyConsumers(new Func<Parser, Type>[] {
                        TypeApp.Consume,
                        NameType.Consume
                    });
                    break;

                default:
                    type = parser.TryConsumer(AtomType.Consume);
                    break;
            }

            if (type == null)
                throw new ParserError(new FailedConsumer(), parser.Cursor);

            return new TypeNode(type, oldCursor);
        }

        public string Pretty(bool printPos = false)
        {
            string prettyPos = "";
            if (printPos) prettyPos = $", pos: {Position.Pretty()}";
            return $"TypeNode({Type.Pretty()}{prettyPos})";
        }
    }
}