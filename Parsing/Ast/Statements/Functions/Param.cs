﻿using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using System.Text;
using Parsing.Ast;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    public class Param : IPrettyPrintable
    {
        public Identifier Name;
        public TypeDescNode Type;

        public Param(Identifier name, TypeDescNode type)
        {
            Name = name;
            Type = type;
        }

        public static Param Consume(Parser parser)
        {
            Identifier name = null;
            TypeDescNode type = null;

            name = parser.TryConsumer(Identifier.Consume);

            parser.Eat(TokenInfo.TokenType.COLON);

            try
            {
                type = parser.TryConsumer(TypeDescNode.Consume);
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected type after COLON token"),
                    parser.Cursor
                );
            }

            return new Param(name, type);
        }

        public string Pretty(int level)
        {
            var sb = new StringBuilder("Param");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            if (Type != null) sb.AppendLine(Display.Utils.Indent(level + 1) + $"Type: {Type.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}
