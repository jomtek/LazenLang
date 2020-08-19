using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazenLang.Typechecking
{
    interface ITypecheckerErrorContent
    { }

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

    class Typechecker
    {
        private Block ast;
        private List<NamespaceName> namespaces;
        private Dictionary<Identifier, Type> environment;

        public Typechecker(Block ast)
        {
            this.ast = ast;
            this.namespaces = new List<NamespaceName>();
            this.environment = new Dictionary<Identifier, Type>();
        }

        public void InferFunction(FuncDecl func)
        {

        }

        private void registerNamespace(InstrNode node)
        {
            var namesp = (NamespaceDecl)node.Value;
            if (!namespaces.Contains(namesp.Name))
            {
                namespaces.Add(namesp.Name);
            }
            else
            {
                throw new TypecheckerError(
                    new NamespaceShadowing(namesp.Name),
                    node.Position
                );
            }
        }

        public void TypecheckAst()
        {
            foreach (InstrNode node in ast.Instructions)
            {
                registerNamespace(node);
            }
        }
    }
}