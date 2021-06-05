using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Types;
using LazenLang.Typechecking.Tools;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking.Checkers
{
    class RegularChecker
    {
        private Environment env;
        private Block block;

        public RegularChecker(Block block, Environment env)
        {
            this.block = block;
            this.env = env;
        }

        public static void CheckVarDecl(InstrNode node, ref Environment env)
        {
            VarDecl variable = (VarDecl)node.Value;
            TypeDesc varType = TypeResolver.ResolveType(variable.Value, env, variable.Value.Position);

            if (variable.Type == null && !(varType is NullType))
                variable.Type = new TypeNode(varType, variable.Value.Position);

            env.AddEntry(variable.Name, varType, node.Position);
        }

        public void Typecheck()
        {

        }
    }
}
