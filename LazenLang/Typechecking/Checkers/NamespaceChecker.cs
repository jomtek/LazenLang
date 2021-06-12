using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.OOP;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Typechecking.Checkers;
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

                if (instruction is VarDecl) RegularChecker.CheckVarDecl(node, ref env);



                if (instruction is ClassDecl)
                {
                    ClassDecl decl = (ClassDecl)instruction;
                    //env.AddEntry(decl.Name, new Class(decl.Typevars.Sequence), node.Position);

                }
            }
        }
    }
}