using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Types.AtomTypes;
using LazenLang.PrettyPrinter;
using LazenLang.Typechecking;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Types
{
    class TypeNode
    {
        public TypeDesc Type { get;  }
        public CodePosition Position { get; }

        public TypeNode(TypeDesc type, CodePosition position)
        {
            Type = type;
            Position = position;
        }

        public static TypeNode Consume(Parser parser)
        {
            CodePosition oldCursor = parser.Cursor;
            TypeDesc type = null;

            switch (parser.LookAhead().Type)
            {
                case TokenInfo.TokenType.L_BRACKET:
                    type = parser.TryConsumer(NativeArrayType.Consume);
                    break;

                case TokenInfo.TokenType.IDENTIFIER:
                    type = parser.TryManyConsumers(new Func<Parser, TypeDesc>[] {
                        TypeAppC.Consume,
                        NameTypeC.Consume
                    });
                    break;

                case TokenInfo.TokenType.FUNC_TYPE:
                    type = parser.TryConsumer(FuncTypeC.Consume);
                    break;

                default:
                    type = parser.TryConsumer(AtomTypeC.Consume);
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
            return $"TypeNode(type: {Type.GetType().Name}{prettyPos})";
        }
    }
}