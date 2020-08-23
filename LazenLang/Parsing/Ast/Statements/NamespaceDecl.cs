using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LazenLang.Parsing.Ast.Statements
{
    class NamespaceName
    {
        public Identifier Seq;
        public NamespaceName(Identifier seq)
        {
            Seq = seq;
        }

        public static NamespaceName Consume(Parser parser)
        {
            string[] portions = Utils.ParseSequence(parser, (Parser p) => Identifier.Consume(p).Value, TokenInfo.TokenType.DOT);
            string name = string.Join('.', portions);

            if (portions.Length == 0)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected name after NAMESPACE token"),
                    parser.Cursor
                );
            }
            return new NamespaceName(new Identifier(name));
        }

        public string Pretty()
        {
            return $"NamespaceName(`{Seq}`)";
        }
    }

    class NamespaceDecl : Instr
    {
        public NamespaceName Name;
        public Block Block;

        public NamespaceDecl(NamespaceName name, Block block)
        {
            Name = name;
            Block = block;
        }

        public static NamespaceDecl Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.NAMESPACE);
            
            NamespaceName name;
            Block block;

            name = parser.TryConsumer(NamespaceName.Consume);

            try
            {
                block = parser.TryConsumer((Parser p) => Block.Consume(p, true, false, true));
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
