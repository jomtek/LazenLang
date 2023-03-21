using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Typechecking.Errors
{
    public interface ITypeCheckerErrorContent
    { }

    public class NameShadowing : ITypeCheckerErrorContent
    {
        public string Name { get; }
        public NameShadowing(string name)
        {
            Name = name;
        }
    }

    public class UnknownName : ITypeCheckerErrorContent
    {
        public string Name { get; }
        public UnknownName(string name)
        {
            Name = name;
        }
    }

    public class TypesMismatched : ITypeCheckerErrorContent
    {
        // Pretty-printed types
        public string Expected { get; }
        public string Got { get; }
        public TypesMismatched(string expected, string got)
        {
            Expected = expected;
            Got = got;
        }
    }
}
