using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LazenLang.Lexing
{
    class Lexer
    {
        public Token[] Tokens;

        public Lexer(string code)
        {
            Tokenize(code);
        }

        private string GetEscapeSequence(char c)
        {
            return "\\u" + ((int)c).ToString("X4");
        }

        private void Tokenize(string code)
        {
            var result = new List<Token>();
            int colTrack = 1;
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

                                
                                if (tokenType != TokenInfo.TokenType.SINGLE_LINE_COMMENT && tokenType != TokenInfo.TokenType.MULTI_LINE_COMMENT)
                                    result.Add(new Token(matchValue.Trim(), tokenType, new CodePosition(lineTrack, colTrack)));
                            }
                        }

                        if (tokenType == TokenInfo.TokenType.EOL)
                        {
                            colTrack = 1;
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

            Tokens = result.ToArray();
        }
    }
}