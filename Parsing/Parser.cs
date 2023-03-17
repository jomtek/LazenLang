using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LazenLang.Lexing;
using Parsing.Errors;

namespace LazenLang.Parsing
{
    public class Parser
    {
        private Token[] Tokens { get; set; }
        private int LookAheadIndex { get; set; }
        public CodePosition Cursor { get; private set; }

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
            if (tokenTypes.Length == 0)
                throw new ArgumentException("Token types list is empty");
            
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
            
            // lastError is never null
            throw lastError!;
        }

        public T TryConsumer<T>(Func<Parser, T> consumer)
        {
            var oldIndex = LookAheadIndex;

            try
            {
                return consumer(this);
            } 
            catch (ParserError)
            {
                LookAheadIndex = oldIndex;
                throw;
            }
        }

        public T TryManyConsumers<T>(Func<Parser, T>[] consumers)
        {
            if (consumers.Length == 0)
                throw new ArgumentException("Consumers list is empty");

            ParserError lastError = null;

            foreach (var consumer in consumers)
            {
                try
                {
                    return TryConsumer(consumer);
                }
                catch (ParserError ex)
                {
                    if (ex.IsExceptionFictive())
                    {
                        lastError = ex;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // lastError is never null
            throw lastError!;
        }

        // Tries many consumers, each one with its condition
        public T TryManyConsumers<T>((bool, Func<Parser, T>)[] entries)
        {
            if (entries.Length == 0)
                throw new ArgumentException("No entries provided");

            ParserError lastError = null;

            foreach (var entry in entries)
            {
                if (!entry.Item1)
                    continue;

                Func<Parser, T> consumer = entry.Item2;
                try
                {
                    return TryConsumer(consumer);
                }
                catch (ParserError ex)
                {
                    if (ex.IsExceptionFictive())
                    {
                        lastError = ex;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // lastError is never null
            throw lastError!;
        }
    }
}