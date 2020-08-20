using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    class GlobalChecker // Top-level typechecker
    {
        private Block ast;
        private GlobalEnvironment globalEnv;

        public GlobalChecker(Block ast)
        {
            this.ast = ast;
            this.globalEnv = new GlobalEnvironment();
        }

        public void Typecheck()
        {
            foreach (InstrNode node in ast.Instructions)
            {
                globalEnv.RegisterNamespace(node);
                var nmspChecker = new NamespaceChecker(((NamespaceDecl)node.Value).Block);
                nmspChecker.Typecheck();
            }
        }
    }
}
