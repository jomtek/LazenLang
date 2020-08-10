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
            InstrNode[] statements = parser.TryConsumer(ParseStatementSeq);
            if (curlyBrackets) parser.Eat(TokenInfo.TokenType.R_CURLY_BRACKET);

            return new Block(statements);
        }

        public override string Pretty()
        {
            return $"Block({InstrNode.PrettyMultiple(Instructions)})";
        }
    }
}