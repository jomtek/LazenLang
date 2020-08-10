using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements
{
    class NamespaceDecl : Instr
    {
        public Identifier Name;
        public Block Block;

        public NamespaceDecl(Identifier name, Block block)
        {
            Name = name;
            Block = block;
        }

        public static NamespaceDecl Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NAMESPACE);
            
            Identifier name;
            Block block;

            try
            {
                name = parser.TryConsumer(Identifier.Consume);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected identifier after NAMESPACE token"),
                    parser.Cursor
                );
            }

            try
            {
                block = parser.TryConsumer((Parser p) => Block.Consume(p, true, true, false));
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block for namespace declaration"),
                    parser.Cursor
                );
            }

            return new NamespaceDecl(name, block);
        }

        public override string Pretty()
        {
            return $"NamespaceDecl(name: {Name.Pretty()}, block: {Block.Pretty()})";
        }

    }
}
