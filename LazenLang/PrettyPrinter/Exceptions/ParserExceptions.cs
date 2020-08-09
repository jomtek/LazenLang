using LazenLang.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.PrettyPrinter
{
    class ParserExceptions
    {
        public static string PrettyContent(IParserErrorContent content)
        {
            switch (content)
            {
                case UnexpectedTokenException x:
                    return x.TokenType.ToString();
                case ExpectedTokenException x:
                    return x.TokenType.ToString();
                case ExpectedElementException x:
                    return x.Message;
                case InvalidElementException x:
                    return x.Message;
                case InvalidDoubleLit x:
                    return $"'{x.Value}'";
                case InvalidCharLit x:
                    return $"'{x.Value}'";
                default:
                    break;
            }

            throw new ArgumentException("content");
        }
    }
}
