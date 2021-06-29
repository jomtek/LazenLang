using LazenLang.Lexing;
using LazenLang.Parsing;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsing.Ast
{
    public abstract class TypeDesc : IPrettyPrintable
    {
        public abstract string Pretty(int level);
    }

    public class TypeDescNode : IPrettyPrintable
    {
        public TypeDesc Value;
        public CodePosition Position;

        public TypeDescNode(TypeDesc value, CodePosition position)
        {
            Value = value;
            Position = position;
        }

        public static TypeDescNode Consume(Parser parser)
        {
            CodePosition oldCursor = parser.Cursor;
            TypeDesc type = null;

            type = parser.LookAhead().Type switch
            {
                TokenInfo.TokenType.L_BRACKET => parser.TryConsumer(NativeArrayType.Consume),
                TokenInfo.TokenType.IDENTIFIER => parser.TryManyConsumers(new Func<Parser, TypeDesc>[] {
                        TypeApp.Consume,
                        NameType.Consume
                }),
                TokenInfo.TokenType.FUNC_TYPE => parser.TryConsumer(FuncType.Consume),
                _ => throw new ParserError(new FailedConsumer(), parser.Cursor),
            };

            return new TypeDescNode(type, oldCursor);
        }

        public string Pretty(int level)
        {
            return "TypeNode: " + Value.Pretty(level);
        }
    }
}