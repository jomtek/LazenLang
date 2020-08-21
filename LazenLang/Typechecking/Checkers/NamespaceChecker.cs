using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Typechecking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    class NamespaceChecker
    {
        private Environment env;
        private List<Identifier> classes;
        private Block block;

        public NamespaceChecker(Block block)
        {
            this.block = block;
            env = new Environment(new Dictionary<Identifier, TypeDesc>());
            classes = new List<Identifier>();
        }

        public void Typecheck()
        {
            foreach (InstrNode node in block.Instructions)
            {
                Instr instruction = node.Value;
                if (instruction is VarDecl)
                {
                    VarDecl variable = (VarDecl)instruction;
                    TypeDesc varType = TypeResolver.ResolveType(variable.Value);
                    env.AddEntry(variable.Name, node.Position, varType);
                }
            }
        }
    }
}