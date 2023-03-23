using LazenLang.Parsing.Ast.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Parsing.Ast
{
    public interface ICreatesSingleBlock
    {
        public Block Block { get; set; }
    }
}
