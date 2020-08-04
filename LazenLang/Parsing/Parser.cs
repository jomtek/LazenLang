using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace LazenLang.Parsing
{
    interface IParserErrorContent { }

    // "Parser class" exceptions
    struct FailedEatToken : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public FailedEatToken(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    struct FailedConsumer : IParserErrorContent
    {}

    struct NoTokenLeft : IParserErrorContent
    {}

    // "Real" exceptions
    struct UnexpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public UnexpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    struct ExpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public ExpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    struct ExpectedElementException : IParserErrorContent
    {
        public string Message { get; }
        public ExpectedElementException(string message)
        {
            Message = message;
        }
    }

    struct InvalidDoubleLit : IParserErrorContent
    {
        public string Value { get; }
        public InvalidDoubleLit(string value)
        {
            Value = value;
        }
    }

    struct InvalidCharLit : IParserErrorContent
    {
        public string Value { get; }
        public InvalidCharLit(string value)
        {
            Value = value;
        }
    }

    // ---
    class ParserError : Exception
    {
        public IParserErrorContent Content;
        public CodePosition Position;

        public ParserError(IParserErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
        }

        public bool IsErrorFromParserClass()
        {
            return Content is FailedEatToken ||
                   Content is NoTokenLeft ||
                   Content is FailedConsumer;
        }
    }

    class Parser
    {
        public List<Token> Tokens { get; set; }
        public Token LastTokenEaten { get; set; }
        public CodePosition Cursor { get; set; }

        public Parser(List<Token> tokens)
        {
            this.Tokens = tokens;
            Cursor = new CodePosition(0, 0);
        }

        public Token Eat(TokenInfo.TokenType tokenType)
        {
            var oldTokens = new Token[Tokens.Count];
            Tokens.CopyTo(oldTokens);

            Token token;

            try
            {
                token = Tokens[0];
                Tokens.RemoveAt(0);
                if (Tokens.Count > 0)
                    Cursor = Tokens[0].Pos;
            } catch (ArgumentOutOfRangeException)
            {
                Tokens = oldTokens.ToList();
                throw new ParserError(new NoTokenLeft(), Cursor);
            }

            if (token.Type == tokenType)
            {
                LastTokenEaten = token;
                return token;
            } else
            {
                Tokens = oldTokens.ToList();
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

        public T TryConsumer<T>(Func<Parser, T> consumer)
        {
            var oldTokens = new Token[Tokens.Count];
            Tokens.CopyTo(oldTokens);

            try
            {
                return consumer(this);
            } catch (ParserError ex)
            {
                Tokens = oldTokens.ToList();
                throw ex;
            }
        }

        public T TryManyConsumers<T>(Func<Parser, T>[] consumers)
        {
            ParserError lastError = null;


            foreach (var consumer in consumers)
            {
                try
                {
                    return TryConsumer(consumer);
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