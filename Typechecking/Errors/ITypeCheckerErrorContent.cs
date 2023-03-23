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
        public string Expected { get; }
        public string Got { get; }
        public TypesMismatched(string expected, string got)
        {
            Expected = expected;
            Got = got;
        }
    }

    public class MultiTypesMismatched : ITypeCheckerErrorContent
    {
        public List<string> Expected { get; }
        public string Got { get; }
        public MultiTypesMismatched(List<string> expected, string got)
        {
            Expected = expected;
            Got = got;
        }
    }

    public class ComparisonProblem : ITypeCheckerErrorContent
    {
        public string Type1 { get; }
        public string Type2 { get; }
        public ComparisonProblem(string type1, string type2)
        {
            Type1 = type1;
            Type2 = type2;
        }
    }

    public class MiscProblem : ITypeCheckerErrorContent
    {
        public string Message { get; }
        public MiscProblem(string message)
        {
            Message = message;
        }
    }
}
