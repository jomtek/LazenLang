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
                Instr statement = null;
                
                try
                {
                    statement = parser.TryConsumer(InstrNode.Consume);
                } catch (ParserError ex)
                {
                    if (!ex.IsErrorFromParserClass())
                        throw ex;
                }

                if (statement == null)
                {
                    try
                    {
                        parser.Eat(TokenInfo.TokenType.EOL);
                    } catch (ParserError)
                    {
                        break;
                    }
                } else
                {
                    statements.Add(statement);
                }
            }

            foreach (Instr statement in statements)
                Console.WriteLine(statement.Pretty());

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