using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.OOP
{
    class ClassDecl : Instr
    {
        public Identifier Name;
        public TypevarSeq Typevars;
        public Block Block;

        public ClassDecl(Identifier name, TypevarSeq typevars, Block block)
        {
            Name = name;
            Typevars = typevars;
            Block = block;
        }

        public static ClassDecl Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.CLASS);

            Identifier name;
            TypevarSeq typevars;
            Block block;

            try
            {
                name = parser.TryConsumer(Identifier.Consume);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected identifier after CLASS token"),
                    parser.Cursor
                );
            }

            typevars = parser.TryConsumer(TypevarSeq.Consume);

            try
            {
                block = parser.TryConsumer((Parser p) => Block.Consume(p, true, false, false, true));
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block for while instruction"),
                    parser.Cursor
                );
            }

            return new ClassDecl(name, typevars, block);
        }

        public override string Pretty()
        {
            return $"ClassDecl(name: {Name.Pretty()}, typevars: {Typevars.Pretty()}, block: {Block.Pretty()})";
        }
    }
}
