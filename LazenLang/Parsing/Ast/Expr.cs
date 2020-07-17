using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast
{
    struct ExprNode
    {
        public Expr Expression;
        public CodePosition Position;

        public ExprNode(Expr expression, CodePosition position)
        {
            Expression = expression;
            Position = position;
        }
    }

    abstract class Expr
    { }
}
