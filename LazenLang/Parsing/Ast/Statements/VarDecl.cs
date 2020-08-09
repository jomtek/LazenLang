using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements
{
    class VarDecl : Instr
    {
        public bool Mutable { get; }
        public Identifier Name { get; }
        public TypeNode Type { get; }
        public ExprNode Value { get; }

        public VarDecl(bool mutable, Identifier name, TypeNode type, ExprNode value)
        {
            Mutable = mutable;
            Name = name;
            Type = type;
            Value = value;
        }

        public static VarDecl Consume(Parser parser)
        {
            bool mutable = true;
            Identifier name = null;
            TypeNode type = null;
            ExprNode value = null;

            Token varOrConstToken = parser.TryManyEats(new TokenInfo.TokenType[]
            {
                TokenInfo.TokenType.VAR,
                TokenInfo.TokenType.CONST
            });

            if (varOrConstToken.Type == TokenInfo.TokenType.CONST)
                mutable = false;

            try
            {
                name = parser.TryConsumer(Identifier.Consume);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.IDENTIFIER),
                    parser.Cursor
                );
            }

            bool colon = true;
            Token colonTok = null;
            try
            {
                colonTok = parser.Eat(TokenInfo.TokenType.COLON);
            } catch (ParserError)
            {
                colon = false;
            }

            if (colon)
            {
                try
                {
                    type = parser.TryConsumer(TypeNode.Consume);
                } catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new ExpectedElementException("Expected type after COLON token"),
                        colonTok.Pos
                    );
                }
            }

            bool assign = true;
            Token assignTok = null;
            try
            {
                assignTok = parser.Eat(TokenInfo.TokenType.ASSIGN);
            } catch (ParserError)
            {
                if (!colon)
                {
                    throw new ParserError(
                        new ExpectedElementException("Expected either type name or value in variable declaration"),
                        parser.Cursor
                    );
                }
                assign = false;
            }

            if (assign)
            {
                try
                {
                    value = parser.TryConsumer(ExprNode.Consume);
                } catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new ExpectedElementException("Expected expression after ASSIGN token"),
                        assignTok.Pos
                    );
                }
            }

            return new VarDecl(mutable, name, type, value);
        }

        public override string Pretty()
        {
            string prettyType = "none", prettyValue = "none";
            if (Type != null) prettyType = Type.Pretty();
            if (Value != null) prettyValue = Value.Value.Pretty();

            return $"VarDecl(mutable: {Mutable}, name: {Name.Pretty()}, type: {prettyType}, value: {prettyValue})";
        }
    }
}