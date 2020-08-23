using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.OOP;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Typechecking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    class NamespaceChecker
    {
        private Environment env;
        private Block block;

        public NamespaceChecker(Block block, Environment env)
        {
            this.block = block;
            this.env = env; 
        }

        public void Typecheck()
        {
            foreach (InstrNode node in block.Instructions)
            {
                Instr instruction = node.Value;
                if (instruction is VarDecl)
                {
                    VarDecl variable = (VarDecl)instruction;
                    TypeDesc varType = TypeResolver.ResolveType(variable.Value, env, variable.Value.Position);

                    if (variable.Type == null && !(varType is NullType))
                        variable.Type = new TypeNode(varType, variable.Value.Position);

                    env.AddEntry(variable.Name, varType, node.Position);
                }

                if (instruction is ClassDecl)
                {
                    ClassDecl classDecl = (ClassDecl)instruction;
                    env.AddEntry(classDecl.Name, new Class(classDecl.Typevars.Sequence), node.Position);
                }
            }
        }
    }
}