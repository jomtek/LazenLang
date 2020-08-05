using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements
{
    class Block
    {
        public Instr[] Instructions { get; set; }

        public Block(Instr[] instructions)
        {
            Instructions = instructions;
        }

        private static Instr[] ParseStatementSeq(Parser parser)
        {
            var statements = new List<Instr>();

            while (true)
            {
                //Console.WriteLine("new cycle------------");
                Instr statement = null;
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
                    try
                    {
                        statement = parser.TryConsumer(InstrNode.Consume);
                    }
                    catch (ParserError ex)
                    {
                        if (!(ex.Content is NoTokenLeft) && !ex.IsExceptionFictive())
                            throw ex;
                        break;
                    }

                    statements.Add(statement);
                }
            }

            return statements.ToArray();
        }

        public static Block Consume(Parser parser, bool curlyBrackets = true)
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
            Instr[] statements = parser.TryConsumer(ParseStatementSeq);
            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.R_CURLY_BRACKET);

            return new Block(statements);
        }
    }
}