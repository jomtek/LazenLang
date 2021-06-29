using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsing.Errors
{
    public class ParserError : Exception
    {
        public IParserErrorContent Content { get; }
        public CodePosition Position { get; }

        public ParserError(IParserErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
        }

        public bool IsExceptionFictive()
        {
            return Content is FailedEatToken ||
                   Content is NoTokenLeft ||
                   Content is FailedConsumer;
        }
    }
}
