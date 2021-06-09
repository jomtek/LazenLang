using System;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Expressions.OOP
{
    // TODO: Is this consumer really worth the cost ?
    public class AttributeAccess : Expr, IPrettyPrintable
    {
        public Expr Left;
        public Expr Right;
        
        public AttributeAccess(Expr left, Expr right)
        {
            Left = left;
            Right = right;
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("AttributeAccess");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Left: {Left.Pretty(level)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Right: {Right.Pretty(level)}");

            return sb.ToString();
        }
    }
}
