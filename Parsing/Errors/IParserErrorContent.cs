using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsing.Errors
{
    public interface IParserErrorContent
    { }

    // "Parser class" exceptions
    public class FailedEatToken : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public FailedEatToken(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    public class FailedConsumer : IParserErrorContent
    { }

    public class NoTokenLeft : IParserErrorContent
    { }

    // "Real" exceptions
    public class UnexpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public UnexpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    public class ExpectedTokenException : IParserErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public ExpectedTokenException(TokenInfo.TokenType tokenType)
        {
            TokenType = tokenType;
        }
    }

    public class ExpectedElementException : IParserErrorContent
    {
        public string Message { get; }
        public ExpectedElementException(string message)
        {
            Message = message;
        }
    }

    public class InvalidElementException : IParserErrorContent
    {
        public string Message { get; }
        public InvalidElementException(string message)
        {
            Message = message;
        }
    }

    public class BannedIdentifier : IParserErrorContent
    {
        public string Value { get; }
        public BannedIdentifier(string value)
        {
            Value = value;
        }
    }

    public class InvalidDoubleLit : IParserErrorContent
    {
        public string Value { get; }
        public InvalidDoubleLit(string value)
        {
            Value = value;
        }
    }

    public class InvalidCharLit : IParserErrorContent
    {
        public string Value { get; }
        public InvalidCharLit(string value)
        {
            Value = value;
        }
    }
}
