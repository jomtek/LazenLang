using LazenLang.Parsing.Ast.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Typechecking
{
    class ClassChecker
    {
        private Environment env;
        private Block block;

        public ClassChecker(Block block, Environment env)
        {
            this.block = block;
            this.env = env;
        }

        public void Typecheck()
        {
        }
    }
}
