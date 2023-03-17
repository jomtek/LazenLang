using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using Parsing.Errors;

namespace LazenLang.Parsing
{
    static class Utils
    {
        public static T[] ParseSequence<T>(Parser parser, Func<Parser, T> consumer, TokenInfo.TokenType delimiter = TokenInfo.TokenType.COMMA)
        {
            var sequence = new List<T>();
            bool lastlyEaten = false;
            Token lastTokenEaten = null;

            while (true)
            {
                try
                {
                    sequence.Add(parser.TryConsumer(consumer));
                    lastlyEaten = false;
                    lastTokenEaten = parser.Eat(delimiter);
                    lastlyEaten = true;
                } catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    break;
                }
            }

            if (lastlyEaten)
            {
                throw new ParserError(
                    new UnexpectedTokenException(delimiter),
                    lastTokenEaten.Pos
                );
            }

            return sequence.ToArray();
        }

        public static Block InstrToBlock(InstrNode instr)
        {
            if (instr.Value is Block)
                return (Block)instr.Value;
            else
                return new Block(new InstrNode[] { instr });
        }

        /*public static TypeDescNode[] ParseGenerics(Parser parser)
        {
            TypeDescNode[] genericArgs = new TypeDescNode[0];

            bool lessToken = true;
            try
            {
                parser.Eat(TokenInfo.TokenType.LESS);
            }
            catch (ParserError)
            {
                lessToken = false;
            }

            if (lessToken)
            {
                genericArgs = Utils.ParseSequence(parser, TypeDescNode.Consume);

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

            return genericArgs;
        }*/
    }
}