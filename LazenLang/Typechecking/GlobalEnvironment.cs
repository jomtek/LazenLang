using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Statements;
using System.Collections.Generic;

namespace LazenLang.Typechecking
{
    class GlobalEnvironment
    {
        public List<NamespaceName> Namespaces { get; }
        public GlobalEnvironment()
        {
            Namespaces = new List<NamespaceName>();
        }

        public void RegisterNamespace(InstrNode node)
        {
            var namesp = (NamespaceDecl)node.Value;
            if (!Namespaces.Contains(namesp.Name))
            {
                Namespaces.Add(namesp.Name);
            }
            else
            {
                throw new TypecheckerError(
                    new NamespaceShadowing(namesp.Name),
                    node.Position
                );
            }
        }
    }
}
