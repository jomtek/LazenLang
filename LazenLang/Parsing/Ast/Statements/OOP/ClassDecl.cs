using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;

namespace LazenLang.Parsing.Ast.Statements.OOP
{
    class ClassDecl : Instr
    {
        public bool PublicAccess { get; }
        public bool Abstract { get; }
        public Identifier Name;
        public TypevarSeq Typevars;
        public Block Block;

        public ClassDecl(bool publicAccess, bool abstract_, Identifier name, TypevarSeq typevars, Block block)
        {
            PublicAccess = publicAccess;
            Abstract = abstract_;
            Name = name;
            Typevars = typevars;
            Block = block;
        }

        public static ClassDecl Consume(Parser parser)
        {
            bool publicAccess = Utils.ParseAccessModifier(parser);
            bool abstract_ = true;
            
            try
            {
                parser.Eat(TokenInfo.TokenType.ABSTRACT);
            } catch (ParserError)
            {
                abstract_ = false;
            }

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
                block = parser.TryConsumer((Parser p) => Block.Consume(p, true, false, true));
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block for while instruction"),
                    parser.Cursor
                );
            }

            return new ClassDecl(publicAccess, abstract_, name, typevars, block);
        }

        public override string Pretty()
        {
            string prettyAccess = PublicAccess ? "public" : "private";
            return $"ClassDecl(access: {prettyAccess}, abstract: {Abstract}, name: {Name.Pretty()}, typevars: {Typevars.Pretty()}, block: {Block.Pretty()})";
        }
    }
}
