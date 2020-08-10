using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions
{
    class FuncCall : Expr
    {
        public Identifier Name { get; }
        public TypeNode[] GenericArgs { get; }
        public ExprNode[] Arguments { get; }

        public FuncCall(Identifier name, TypeNode[] genericArgs, ExprNode[] arguments)
        {
            Name = name;
            GenericArgs = genericArgs;
            Arguments = arguments;
        }

        public static FuncCall Consume(Parser parser)
        {
            Identifier name = null;
            TypeNode[] genericArgs = null;
            ExprNode[] arguments = null;

            name = parser.TryConsumer(Identifier.Consume);

            bool lessToken = true;
            try
            {
                parser.Eat(TokenInfo.TokenType.LESS);
            } catch (ParserError)
            {
                lessToken = false;
            }

            if (lessToken)
            {
                genericArgs = Utils.ParseSequence(parser, TypeNode.Consume);

                try
                {
                    parser.Eat(TokenInfo.TokenType.GREATER);
                }
                catch (ParserError ex)
                {
                    if (genericArgs.Count() > 1)
                    {
                        throw new ParserError(
                            new ExpectedTokenException(TokenInfo.TokenType.GREATER),
                            parser.Cursor
                        );
                    }

                    throw ex;
                }

                if (genericArgs.Count() == 0)
                {
                    throw new ParserError(
                        new ExpectedElementException("Expected one or more generic types"),
                        parser.Cursor
                    );
                }
            }

            parser.Eat(TokenInfo.TokenType.L_PAREN);

            arguments = Utils.ParseSequence(parser, ExprNode.Consume);

            try
            {
                parser.Eat(TokenInfo.TokenType.R_PAREN);
            }
            catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.R_PAREN),
                    parser.Cursor
                );
            }

            return new FuncCall(name, genericArgs, arguments);
        }

        public override string Pretty()
        {
            string prettyGenericArgs = "";
            string prettyArgs = "";

            if (GenericArgs != null)
            {
                for (int i = 0; i < GenericArgs.Count(); i++)
                {
                    TypeNode node = GenericArgs[i];
                    prettyGenericArgs += node.Pretty();
                    if (i != GenericArgs.Count() - 1) prettyGenericArgs += ", ";
                }
            }

            if (Arguments != null)
            {
                for (int i = 0; i < Arguments.Count(); i++)
                {
                    ExprNode node = Arguments[i];
                    prettyArgs += node.Pretty();
                    if (i != Arguments.Count() - 1) prettyArgs += ", ";
                }
            }

            return $"FuncCall(name: {Name.Pretty()}, genericArgs: {{{prettyGenericArgs}}}, args: {{{prettyArgs}}})";
        }
    }
}
