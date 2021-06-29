using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Parsing.Display;
using Parsing.Ast;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements
{
    public class VarDecl : Instr, IPrettyPrintable
    {
        public bool Mutable { get; }
        public bool PublicAccess { get; }
        public bool Static { get;  }
        public Identifier Name { get; }
        public TypeDescNode Type { get; set; }
        public ExprNode Value { get; }

        public VarDecl(bool mutable, bool publicAccess, bool static_, Identifier name, TypeDescNode type, ExprNode value)
        {
            Mutable = mutable;
            PublicAccess = publicAccess;
            Static = static_;
            Name = name;
            Type = type;
            Value = value;
        }

        public static VarDecl Consume(Parser parser, bool allowAccessModifier = true, bool allowStatic = true, bool allowValue = true)
        {
            bool mutable = true;
            bool publicAccess = true;
            bool static_ = false;
            Identifier name = null;
            TypeDescNode type = null;
            ExprNode value = null;

            if (allowAccessModifier) publicAccess = Utils.ParseAccessModifier(parser);
            if (allowStatic) static_ = Utils.ParseStatic(parser);
            
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
            }
            catch (ParserError)
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
            }
            catch (ParserError)
            {
                colon = false;
            }

            if (colon)
            {
                try
                {
                    type = parser.TryConsumer(TypeDescNode.Consume);
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new ExpectedElementException("Expected type after COLON token"),
                        colonTok.Pos
                    );
                }
            }

            if (allowValue)
            {
                bool assign = true;
                Token assignTok = null;
                try
                {
                    assignTok = parser.Eat(TokenInfo.TokenType.ASSIGN);
                }
                catch (ParserError)
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
                    }
                    catch (ParserError ex)
                    {
                        if (!ex.IsExceptionFictive()) throw ex;
                        throw new ParserError(
                            new ExpectedElementException("Expected expression after ASSIGN token"),
                            assignTok.Pos
                        );
                    }
                }
            }

            if (value == null)
            {
                if (type == null)
                {
                    throw new ParserError(
                        new ExpectedElementException("Expected either type name or value in variable declaration"),
                        parser.Cursor
                    );
                }
                else if (!mutable)
                {
                    throw new ParserError(
                       new InvalidElementException("Please specify a value for your constant"),
                       parser.Cursor
                    );
                }
            }

            return new VarDecl(mutable, publicAccess, static_, name, type, value);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("VarDecl");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Mutable: {Mutable}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Static: {Static}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Public Access: {PublicAccess}");
            if (Type != null) sb.AppendLine(Display.Utils.Indent(level + 1) + $"Type: {Type.Pretty(level + 1)}");
            if (Value != null) sb.AppendLine(Display.Utils.Indent(level + 1) + $"Value: {Value.Pretty(level + 1)}");
            
            return sb.ToString();
        }
    }
}