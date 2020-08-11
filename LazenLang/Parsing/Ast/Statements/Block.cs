using System;
using System.Collections.Generic;
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

        private static InstrNode[] ParseStatementSeq(Parser parser, bool topLevelLike, bool topLevel)
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

                        throw new ParserError(
                            new UnexpectedTokenException(nextTokenType),
                            parser.LookAhead().Pos
                        );
                    }

                    try
                    {
                        if (topLevel)
                        {
                            statement = parser.TryConsumer((Parser p) => new InstrNode(p.TryConsumer(NamespaceDecl.Consume), p.Cursor));
                        }
                        else if (topLevelLike)
                        {
                            statement = parser.TryManyConsumers(new Func<Parser, InstrNode>[]{
                                (Parser p) => new InstrNode(p.TryConsumer(VarDecl.Consume), p.Cursor),
                                (Parser p) => new InstrNode(p.TryConsumer(FuncDecl.Consume), p.Cursor),
                                (Parser p) => new InstrNode(p.TryConsumer(ClassDecl.Consume), p.Cursor)
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
                        if (topLevelLike && parser.LookAhead().Type != TokenInfo.TokenType.R_CURLY_BRACKET)
                        {
                            throw new ParserError(
                                new InvalidElementException("Unwanted instruction"),
                                parser.Cursor
                            );
                        }
                        break;
                    }

                    statements.Add(statement);
                    isLastEOL = false;
                } else
                {
                    isLastEOL = true;
                }
            }

            return statements.ToArray();
        }

        public static Block Consume(Parser parser, bool curlyBrackets = true, bool topLevelLike = false, bool topLevel = false)
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
            InstrNode[] statements = parser.TryConsumer((Parser p) => ParseStatementSeq(parser, topLevelLike, topLevel));
            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.R_CURLY_BRACKET);

            return new Block(statements);
        }

        public override string Pretty()
        {
            return $"Block({InstrNode.PrettyMultiple(Instructions)})";
        }
    }
}