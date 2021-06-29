using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Display
{
    public interface IPrettyPrintable
    {
        public string Pretty(int level);
    }
}
