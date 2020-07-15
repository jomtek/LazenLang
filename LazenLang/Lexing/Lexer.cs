using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LazenLang.Lexing
{
    class Lexer
    {
        public List<Token> tokens = new List<Token>();

        public Lexer(string code)
        {
            tokenize(code);
        }

        private string GetEscapeSequence(char c)
        {
            return "\\u" + ((int)c).ToString("X4");
        }

        private void tokenize(string code)
        {
            int colTrack = 0;
            int lineTrack = 1;

            while (code.Length > 0)
            {
                foreach ((string, TokenInfo.TokenType) regexPair in TokenInfo.RegexTable)
                {
                    TokenInfo.TokenType tokenType = regexPair.Item2;

                    var regex = new Regex(regexPair.Item1);
                    var match = regex.Match(code);

                    if (match.Success)
                    {
                        if (tokenType != TokenInfo.TokenType.SPACE && tokenType != TokenInfo.TokenType.TAB)
                            if (GetEscapeSequence(match.Value[0]) != @"\u000D") // Because we don't need CR characters as tokens
                                tokens.Add(new Token(match.Value.Trim(), tokenType, new CodePosition(lineTrack, colTrack)));

                        if (tokenType == TokenInfo.TokenType.EOL)
                        {
                            colTrack = 0;
                            lineTrack++;
                        } else
                        {
                            colTrack += match.Length;
                        }

                        code = code.Substring(match.Length);
                        break;
                    }
                }
            }
        }
    }
}