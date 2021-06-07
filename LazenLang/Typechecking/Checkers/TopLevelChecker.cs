using LazenLang.Parsing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    class TopLevelChecker // Top-level typechecker
    {
        private Block ast;
        private Environment topLevelEnvironment;

        public TopLevelChecker(Block ast)
        {
            this.ast = ast;
            this.topLevelEnvironment = new Environment(new Dictionary<Identifier, TypeDesc>());
        }

        public void Typecheck()
        {
            foreach (InstrNode node in ast.Instructions)
            {
                //NamespaceDecl nmsp = ((NamespaceDecl)node.Value);
                //topLevelEnvironment.AddEntry(nmsp.Name.Seq, new Namespace(), node.Position);
                //new NamespaceChecker(nmsp.Block, topLevelEnvironment).Typecheck();
            }
        }
    }
}