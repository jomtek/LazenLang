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

    class IdentifierShadowing : ITypecheckerErrorContent
    {
        public Identifier Identifier;
        public IdentifierShadowing(Identifier identifier)
        {
            Identifier = identifier;
        }
    }

    class NamespaceShadowing : ITypecheckerErrorContent
    {
        public NamespaceName NamespaceName;
        public NamespaceShadowing(NamespaceName namespaceName)
        {
            NamespaceName = namespaceName;
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