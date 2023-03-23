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

            while (true)
            {
                InstrNode statement = null;
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
                            (Parser p) => new InstrNode(p.TryConsumer((Parser _) => FuncDecl.Consume(p)), p.Cursor),
                            (Parser p) => p.TryConsumer((Parser _) => InstrNode.Consume(p)),
                        });
                    }
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    if (
                        !(ex.Content is NoTokenLeft) && 
                        parser.LookAhead().Type != TokenInfo.TokenType.R_CURLY_BRACKET)
                    {
                        throw new ParserError(
                            new UnexpectedTokenException(parser.LookAhead().Type),
                            parser.Cursor
                        );
                    }
                    break;
                }

                if (parser.LookBefore().Type != TokenInfo.TokenType.R_CURLY_BRACKET)
                {
                    try
                    {
                        parser.Eat(TokenInfo.TokenType.SEMI_COLON);
                    }
                    catch (ParserError)
                    {
                        throw new ParserError(
                            new UnexpectedTokenException(parser.LookAhead().Type),
                            parser.Cursor
                        );
                    }

                }

                statements.Add(statement);
            }

            return statements.ToArray();
        }

        public static Block Consume(Parser parser, bool curlyBrackets = true, bool topLevel = false, bool intoClass = false)
        {
            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.L_CURLY_BRACKET, false);

            InstrNode[] statements = parser.TryConsumer((Parser p) => ParseStatementSeq(parser, topLevel, intoClass));

            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.R_CURLY_BRACKET, false);
            
            return new Block(statements);
        }

        public override string Pretty(int level)
        {
            return "Block: " + Display.Utils.PrettyArray(Instructions, level);
        }
    }
}