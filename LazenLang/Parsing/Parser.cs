using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace LazenLang.Parsing
{
    interface ParseErrorContent { }

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

    class ParseError : Exception
    {
        public ParseErrorContent Content;
        public CodePosition Position;

        public ParseError(ParseErrorContent content, CodePosition position)
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
                throw new ParseError(new NoTokenLeft(), new CodePosition(-1, -1));
            }

            if (token.Type == tokenType)
            {
                return token;
            } else
            {
                tokens = oldTokens;
                throw new ParseError(new FailedEatToken(token.Type), token.Pos);
            }
        }
    }
}
