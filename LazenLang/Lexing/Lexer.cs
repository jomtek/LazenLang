using System;
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
                if (GetEscapeSequence(code[0]) == @"\u000D")
                {
                    code = code.Substring(1);
                    continue;
                }

                foreach ((string, TokenInfo.TokenType) regexPair in
                        Char.IsLetter(code[0]) ? TokenInfo.IdenRegexTable : TokenInfo.OtherRegexTable)
                {
                    TokenInfo.TokenType tokenType = regexPair.Item2;
                 
                    Match match = new Regex(regexPair.Item1).Match(code);
                    if (match.Success)
                    {
                        int matchLength = match.Length;
                        string matchValue = match.Value;

                        if (tokenType != TokenInfo.TokenType.SPACE && tokenType != TokenInfo.TokenType.TAB)
                        {
                            if (tokenType == TokenInfo.TokenType.STRING_LIT || tokenType == TokenInfo.TokenType.CHAR_LIT)
                                matchValue = matchValue.Substring(1).Remove(matchLength - 2);

                            if (tokenType != TokenInfo.TokenType.SINGLE_LINE_COMMENT && tokenType != TokenInfo.TokenType.MULTI_LINE_COMMENT)
                                result.Add(new Token(matchValue.Trim(), tokenType, new CodePosition(lineTrack, colTrack)));
                        }

                        if (tokenType == TokenInfo.TokenType.EOL)
                        {
                            colTrack = 1;
                            lineTrack++;
                        }
                        else
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