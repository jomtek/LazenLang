using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LazenLang.Parsing
{
    interface IParserErrorContent
    { }

    // "Parser class" exceptions
    class FailedEatToken : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public FailedEatToken(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    class FailedConsumer : IParserErrorContent
    { }

    class NoTokenLeft : IParserErrorContent
    { }

    // "Real" exceptions
    class UnexpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public UnexpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    class ExpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public ExpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    class ExpectedElementException : IParserErrorContent
    {
        public string Message { get; }
        public ExpectedElementException(string message)
        {
            Message = message;
        }
    }

    class InvalidElementException : IParserErrorContent
    {
        public string Message { get; }
        public InvalidElementException(string message)
        {
            Message = message;
        }
    }

    class BannedIdentifier : IParserErrorContent
    {
        public string Value { get; }
        public BannedIdentifier(string value)
        {
            Value = value;
        }
    }

    class InvalidDoubleLit : IParserErrorContent
    {
        public string Value { get; }
        public InvalidDoubleLit(string value)
        {
            Value = value;
        }
    }

    class InvalidCharLit : IParserErrorContent
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
        public IParserErrorContent Content { get; }
        public CodePosition Position { get; }

        public ParserError(IParserErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
        }

        public bool IsExceptionFictive()
        {
            return Content is FailedEatToken ||
                   Content is NoTokenLeft ||
                   Content is FailedConsumer;
        }
    }
    
    public class Parser
    {
        private Token[] Tokens { get; set; }
        private int LookAheadIndex { get; set; }
        public CodePosition Cursor { get; set; }

        public Parser(Token[] tokens)
        {
            Tokens = tokens;
            LookAheadIndex = 0;
            Cursor = new CodePosition(0, 0);
        }

        public Token LookAhead()
        {
            if (LookAheadIndex >= Tokens.Length)
                throw new ParserError(new NoTokenLeft(), Cursor);
            return Tokens[LookAheadIndex];
        }

        public bool AreTokensRemaining()
        {
            return LookAheadIndex < Tokens.Count();
        }

        public static Func<Parser, Token> CurryEat(TokenInfo.TokenType tokenType) =>
            (Parser p) => p.Eat(tokenType);

        public Token Eat(TokenInfo.TokenType tokenType, bool facultative = true)
        {
            if (LookAhead().Type != tokenType)
            {
                if (facultative)
                    throw new ParserError(new FailedEatToken(tokenType), Cursor);
                else
                    throw new ParserError(new ExpectedTokenException(tokenType), Cursor);
            }
            else if (Tokens.Length == 0)
            {
                throw new ParserError(new NoTokenLeft(), Cursor);
            }

            if (LookAhead().Type != TokenInfo.TokenType.EOL)
                Cursor = LookAhead().Pos;
            else
                Cursor = new CodePosition(LookAhead().Pos.Line + 1, 1);

            LookAheadIndex++;
            return Tokens[LookAheadIndex - 1];
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
            int oldIndex = LookAheadIndex;

            try
            {
                return consumer(this);
            } catch (ParserError ex)
            {
                LookAheadIndex = oldIndex;
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
                    if (ex.IsExceptionFictive())
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