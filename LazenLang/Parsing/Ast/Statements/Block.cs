using System.Collections.Generic;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements
{
    class Block : Instr
    {
        public InstrNode[] Instructions { get; }

        public Block(InstrNode[] instructions)
        {
            Instructions = instructions;
        }

        private static InstrNode[] ParseStatementSeq(Parser parser)
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
                    if (!isLastEOL) break;
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
                    isLastEOL = false;
                } else
                {
                    isLastEOL = true;
                }
            }

            return statements.ToArray();
        }

        public static Block Consume(Parser parser, bool curlyBrackets = true, bool topLevel = false)
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
            InstrNode[] statements = parser.TryConsumer(ParseStatementSeq);
            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.R_CURLY_BRACKET);

            if (topLevel && parser.AreTokensRemaining())
            {
                throw new ParserError(
                    new UnexpectedTokenException(parser.LookAhead().Type),
                    parser.Cursor
                );
            }

            return new Block(statements);
        }

        public override string Pretty()
        {
            return $"Block({InstrNode.PrettyMultiple(Instructions)})";
        }
    }
}