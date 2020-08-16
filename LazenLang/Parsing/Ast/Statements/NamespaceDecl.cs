using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LazenLang.Parsing.Ast.Statements
{
    class NamespaceName
    {
        public Identifier[] Portions;
        public NamespaceName(Identifier[] portions)
        {
            Portions = portions;
        }

        public static NamespaceName Consume(Parser parser)
        {
            Identifier[] portions = Utils.ParseSequence(parser, Identifier.Consume, TokenInfo.TokenType.DOT);
            if (portions.Length == 0)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected name after NAMESPACE token"),
                    parser.Cursor
                );
            }
            return new NamespaceName(portions);
        }

        public string Pretty()
        {
            IEnumerable<string> portions = (from x in Portions select x.Value);
            return $"NamespaceName(`{string.Join('.', portions)}`)";
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
