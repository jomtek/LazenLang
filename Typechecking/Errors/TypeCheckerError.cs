using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Typechecking.Errors
{
    public class TypeCheckerError : Exception
    {
        public ITypeCheckerErrorContent Content { get; }
        public CodePosition Position { get; }

        public TypeCheckerError(ITypeCheckerErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
        }
    }
}
