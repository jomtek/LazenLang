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
        public Identifier Name { get; }
        public TypeDescNode Type { get; set; }
        public ExprNode Value { get; }

        public VarDecl(Identifier name, TypeDescNode type, ExprNode value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

        public static VarDecl Consume(Parser parser, bool allowValue = true)
        {
            Identifier name = null;
            TypeDescNode type = null;
            ExprNode value = null;
            
            name = parser.TryConsumer(Identifier.Consume);

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
                        if (!ex.IsExceptionFictive()) throw;
                        throw new ParserError(
                            new ExpectedElementException("Expected expression after ASSIGN token"),
                            assignTok.Pos
                        );
                    }
                }
            }

            return new VarDecl(name, type, value);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("VarDecl");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Name: {Name.Pretty(level + 1)}");
            if (Type != null) sb.AppendLine(Display.Utils.Indent(level + 1) + $"Type: {Type.Pretty(level + 1)}");
            if (Value != null) sb.AppendLine(Display.Utils.Indent(level + 1) + $"Value: {Value.Pretty(level + 1)}");
            
            return sb.ToString();
        }
    }
}