using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace LazenLang.Parsing
{
    interface ParserErrorContent { }

    // "Parser class" exceptions
    struct InvalidCharLit : ParserErrorContent
    {
        public string Literal { get; }
        public InvalidCharLit(string literal)
        {
            Literal = literal;
        }
    }

    struct FailedEatToken : ParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public FailedEatToken(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    struct FailedConsumer : ParserErrorContent
    {}

    struct NoTokenLeft : ParserErrorContent
    {}

    // "Real" exceptions
    struct UnexpectedTokenException : ParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get;  }
        public UnexpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    // ---
    class ParserError : Exception
    {
        public ParserErrorContent Content;
        public CodePosition Position;

        public ParserError(ParserErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
        }

        public bool IsErrorFromParserClass()
        {
            return Content is InvalidCharLit ||
                   Content is FailedEatToken ||
                   Content is NoTokenLeft ||
                   Content is FailedConsumer;
        }
    }

    class Parser
    {
        public List<Token> tokens { get; set; }
        public CodePosition cursor;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
            cursor = new CodePosition(0, 0);
        }

        public Token Eat(TokenInfo.TokenType tokenType)
        {
            var oldTokens = new Token[tokens.Count];
            tokens.CopyTo(oldTokens);

            Token token;

            try
            {
                token = tokens[0];
                tokens.RemoveAt(0);
                if (tokens.Count > 0)
                    cursor = tokens[0].Pos;
            } catch (ArgumentOutOfRangeException)
            {
                tokens = oldTokens.ToList();
                throw new ParserError(new NoTokenLeft(), cursor);
            }

            if (token.Type == tokenType)
            {
                return token;
            } else
            {
                tokens = oldTokens.ToList();
                throw new ParserError(new FailedEatToken(token.Type), token.Pos);
            }
        }

        public Token TryManyEats(TokenInfo.TokenType[] tokenTypes)
        {
            ParserError lastError = null;

            foreach (TokenInfo.TokenType tokType in tokenTypes)
            {
                try
                {
                    return Eat(tokType);
                } catch (ParserError ex)
                {
                    lastError = ex;
                }
            }

            throw lastError;
        }

        public T TryConsumer<T>(Func<Parser, T> consumer, Parser parser)
        {
            var oldTokens = new Token[tokens.Count];
            tokens.CopyTo(oldTokens);

            try
            {
                return consumer(parser);
            } catch (ParserError ex)
            {
                parser.tokens = oldTokens.ToList();
                throw ex;
            }
        }

        public T TryManyConsumers<T>(Func<Parser, T>[] consumers, Parser parser)
        {
            ParserError lastError = null;


            foreach (var consumer in consumers)
            {
                try
                {
                    return TryConsumer(consumer, parser);
                } catch (ParserError ex)
                {
                    if (ex.IsErrorFromParserClass())
                    {
                        lastError = ex;
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            throw lastError;
        }
    }
}
