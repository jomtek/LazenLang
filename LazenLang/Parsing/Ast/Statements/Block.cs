using System;
using System.Collections.Generic;
using System.Linq;
using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Parsing.Ast.Statements.OOP;

namespace LazenLang.Parsing.Ast.Statements
{
    class Block : Instr
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
                bool eolFailed = false;

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
                        if (topLevel)
                        {
                            statement = parser.TryManyConsumers(new Func<Parser, InstrNode>[]{
                                (Parser p) => new InstrNode(p.TryConsumer(ClassDecl.Consume), p.Cursor)
                            });
                        }
                        else if (intoClass)
                        {
                            Console.WriteLine("into class");
                            statement = parser.TryManyConsumers(new Func<Parser, InstrNode>[]{
                                (Parser p) => new InstrNode(p.TryConsumer((Parser p) => VarDecl.Consume(p, true, true)), p.Cursor),
                                (Parser p) => new InstrNode(p.TryConsumer((Parser p) => FuncDecl.Consume(p, true)), p.Cursor),
                                (Parser p) => new InstrNode(p.TryConsumer(ConstructorDecl.Consume), p.Cursor)
                            });
                        }
                        else
                        {
                            statement = parser.TryConsumer(InstrNode.Consume);
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

            if (!intoClass)
            {
                //Console.WriteLine("ye");
            }

            return new Block(statements);
        }

        public override string Pretty()
        {
            return $"Block({InstrNode.PrettyMultiple(Instructions)})";
        }
    }
}