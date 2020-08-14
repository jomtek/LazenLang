using System;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Expressions.OOP
{
    class AttributeAccess : Expr
    {
        public Expr Left;
        public Expr Right;
        
        public AttributeAccess(Expr left, Expr right)
        {
            Left = left;
            Right = right;
        }

        public override string Pretty()
        {
            return $"AttributeAccess(left: {Left.Pretty()}, right: {Right.Pretty()})";
        }
    }
}
