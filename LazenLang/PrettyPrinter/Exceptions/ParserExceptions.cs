using LazenLang.Parsing;
using System;

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
                case BannedIdentifier x:
                    return $"`{x.Value}`";
                case InvalidDoubleLit x:
                    return $"'{x.Value}'";
                case InvalidCharLit x:
                    return $"'{x.Value}'";
                default:
                    break;
            }

            Console.WriteLine(content.GetType());
            throw new ArgumentException("content");
        }
    }
}
