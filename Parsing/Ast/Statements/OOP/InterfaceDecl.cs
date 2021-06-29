using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.OOP
{
    class InterfaceDecl : Instr, IPrettyPrintable
    {
        public bool PublicAccess { get; }
        public Identifier Name;
        public TypevarSeq Typevars;
        public Block Block;

        public InterfaceDecl(bool publicAccess, Identifier name, TypevarSeq typevars, Block block)
        {
            PublicAccess = publicAccess;
            Name = name;
            Typevars = typevars;
            Block = block;
        }

        public static InterfaceDecl Consume(Parser parser)
        {
            bool publicAccess = Utils.ParseAccessModifier(parser);

            parser.Eat(TokenInfo.TokenType.INTERFACE);

            Identifier name;
            TypevarSeq typevars;
            Block block;

            try
            {
                name = parser.TryConsumer(Identifier.Consume);
            }
            catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected identifier after INTERFACE token"),
                    parser.Cursor
                );
            }

            typevars = parser.TryConsumer(TypevarSeq.Consume);

            try
            {
                block = parser.TryConsumer((Parser p) => Block.Consume(p, true, false, false, true));
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block for interface declaration"),
                    parser.Cursor
                );
            }

            return new InterfaceDecl(publicAccess, name, typevars, block);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("InterfaceDecl");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"PublicAccess: {PublicAccess}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Typevars: {Display.Utils.PrettyArray(Typevars.Sequence, level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"{Block.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}
