using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;
using System;

namespace LazenLang.Typechecking
{
    interface ITypecheckerErrorContent
    { }

    class MismatchedTypes : ITypecheckerErrorContent
    {
        public TypeDesc Expected;
        public TypeDesc Found;
        public MismatchedTypes(TypeDesc expected, TypeDesc found)
        {
            Expected = expected;
            Found = found;
        }
    }

    class EnvironmentError : ITypecheckerErrorContent
    {
        public string Message;
        public EnvironmentError(string message)
        {
            Message = message;
        }
    }

    // ---
    class TypecheckerError : Exception
    {
        public ITypecheckerErrorContent Content;
        public CodePosition Position;

        public TypecheckerError(ITypecheckerErrorContent content, CodePosition position)
        {
            Content = content;
            Position = position;
        }
    }
}