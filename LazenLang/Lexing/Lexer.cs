using System;
using System.Collections.Generic;
using System.Linq;
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
                    Match match = regex.Match(code);
                    int matchLength = match.Length;
                    string matchValue = match.Value;

                    if (match.Success)
                    {
                        if (tokenType != TokenInfo.TokenType.SPACE && tokenType != TokenInfo.TokenType.TAB)
                        {
                            if (GetEscapeSequence(matchValue[0]) != @"\u000D") // Because we don't need CR characters as tokens
                            {
                                if (tokenType == TokenInfo.TokenType.STRING_LIT || tokenType == TokenInfo.TokenType.CHAR_LIT)
                                    matchValue = matchValue.Substring(1).Remove(matchLength - 2);

                                tokens.Add(new Token(matchValue.Trim(), tokenType, new CodePosition(lineTrack, colTrack)));
                            }
                        }

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