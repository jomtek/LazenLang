using System;
using System.Collections.Generic;
using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Parsing.Ast.Statements.OOP;
using LazenLang.Parsing.Display;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Statements
{
    public class Block : Instr, IPrettyPrintable
    {
        public InstrNode[] Instructions { get; }

        public Block(InstrNode[] instructions)
        {
            Instructions = instructions;
        }

        private static InstrNode[] ParseStatementSeq(Parser parser, bool topLevel, bool intoClass)
        {
            var statements = new List<InstrNode>();
            bool isLastEOL = true;

            while (true)
            {
                InstrNode statement = null;
                var eolFailed = false;

                try
                {
                    parser.Eat(TokenInfo.TokenType.EOL);
                }
                catch (ParserError)
                {
                    eolFailed = true;
                }

                if (eolFailed)
                {
                    if (!isLastEOL)
                    {
                        TokenInfo.TokenType nextTokenType;
                        try
                        {
                            nextTokenType = parser.LookAhead().Type;
                        } catch (ParserError)
                        {
                            break;
                        }

                        if (nextTokenType == TokenInfo.TokenType.R_CURLY_BRACKET)
                            break;

                        throw new ParserError(
                            new UnexpectedTokenException(nextTokenType),
                            parser.LookAhead().Pos
                        );
                    }

                    try
                    {
                        if (intoClass)
                        {
                            statement = parser.TryManyConsumers(new Func<Parser, InstrNode>[]{
                                (Parser p) => new InstrNode(p.TryConsumer((Parser _) => VarDecl.Consume(p)), p.Cursor),
                                (Parser p) => new InstrNode(p.TryConsumer((Parser _) => FuncDecl.Consume(p)), p.Cursor)
                            });
                        }
                        else
                        {
                            statement = parser.TryManyConsumers(new Func<Parser, InstrNode>[]{
                                (Parser p) => new InstrNode(p.TryConsumer((Parser _) => ClassDecl.Consume(p)), p.Cursor),
                                (Parser p) => p.TryConsumer((Parser _) => InstrNode.Consume(p)),
                            });
                        }
                    }
                    catch (ParserError ex)
                    {
                        if (!ex.IsExceptionFictive()) throw ex;
                        if (parser.LookAhead().Type != TokenInfo.TokenType.R_CURLY_BRACKET)
                        {
                            throw new ParserError(
                                new UnexpectedTokenException(parser.LookAhead().Type),
                                parser.Cursor
                            );
                        }
                        break;
                    }

                    statements.Add(statement);
                    isLastEOL = false;
                }
                else
                {
                    isLastEOL = true;
                }
            }

            return statements.ToArray();
        }

        public static Block Consume(Parser parser, bool curlyBrackets = true, bool topLevel = false, bool intoClass = false)
        {
            while (true)
            {
                try
                {
                    parser.Eat(TokenInfo.TokenType.EOL);
                } catch (ParserError)
                {
                    break;
                }
            }

            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.L_CURLY_BRACKET);

            InstrNode[] statements = parser.TryConsumer((Parser p) => ParseStatementSeq(parser, topLevel, intoClass));

            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.R_CURLY_BRACKET);
            
            return new Block(statements);
        }

        public override string Pretty(int level)
        {
            return "Block: " + Display.Utils.PrettyArray(Instructions, level);
        }
    }
}