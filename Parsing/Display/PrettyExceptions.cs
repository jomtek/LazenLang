using Parsing.Errors;
using System;

namespace LazenLang.Parsing.Display
{
    public static class PrettyExceptions
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

            Console.WriteLine(content.GetType());
            throw new ArgumentException("content");
        }
    }
}
