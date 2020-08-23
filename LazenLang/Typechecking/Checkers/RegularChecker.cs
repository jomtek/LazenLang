using LazenLang.Parsing.Ast.Statements;
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

        public void Typecheck()
        {

        }
    }
}
