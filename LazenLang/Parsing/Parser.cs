using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace LazenLang.Parsing
{
    interface ParseErrorContent { }

    struct InvalidCharLit : ParseErrorContent
    {
        public string Literal { get; }
        public InvalidCharLit(string literal)
        {
            Literal = literal;
        }
    }

    struct FailedEatToken : ParseErrorContent
    {
        public TokenInfo.TokenType TokenType { get; }
        public FailedEatToken(TokenInfo.TokenType token)
        {
            TokenType = token;
        }
    }

    struct NoTokenLeft : ParseErrorContent
    {}

    class ParserError : Exception
    {
        public ParseErrorContent Content;
        public CodePosition Position;

        public ParserError(ParseErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
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

        public bool IsErrorFromParserClass(ParserError error)
        {
            ParseErrorContent content = error.Content;
            return content is InvalidCharLit ||
                   content is FailedEatToken ||
                   content is NoTokenLeft;
        }

        public Token Eat(TokenInfo.TokenType tokenType)
        {
            var oldTokens = tokens;
            Token token;

            try
            {
                token = tokens[0];
                tokens.RemoveAt(0);
                if (tokens.Count > 0)
                    cursor = tokens[0].Pos;
            } catch (IndexOutOfRangeException)
            {
                tokens = oldTokens;
                throw new ParserError(new NoTokenLeft(), new CodePosition(-1, -1));
            }

            if (token.Type == tokenType)
            {
                return token;
            } else
            {
                tokens = oldTokens;
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
            List<Token> oldTokens = parser.tokens;

            try
            {
                return consumer(parser);
            } catch (ParserError ex)
            {
                parser.tokens = oldTokens;
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
                    if (IsErrorFromParserClass(ex))
                        lastError = ex;
                    else
                        throw ex;
                }
            }

            throw lastError;
        }
    }
}
