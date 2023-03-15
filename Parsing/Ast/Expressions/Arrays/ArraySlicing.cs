using LazenLang.Parsing.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions.Arrays
{
    /*public class ArraySlicing : Expr, IPrettyPrintable
    {
        public Expr Expr { get; }
        public ExprNode Beginning { get; }
        public ExprNode Ending { get; }
        public ExprNode Step { get; }
        public bool IsIndexing { get; }

        public ArraySlicing(Expr expr, ExprNode beginning, ExprNode ending, ExprNode step, bool isIndexing)
        {
            Expr = expr;
            Beginning = beginning;
            Ending = ending;
            Step = step;
            IsIndexing = isIndexing;
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("ArraySlicing");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Expr: {Expr.Pretty(level + 1)}");
            if (Beginning != null)
                sb.AppendLine(Display.Utils.Indent(level + 1) + $"Beginning: {Beginning.Pretty(level + 1)}");
            if (Ending != null)
                sb.AppendLine(Display.Utils.Indent(level + 1) + $"Ending: {Ending.Pretty(level + 1)}");
            if (Step != null)
                sb.AppendLine(Display.Utils.Indent(level + 1) + $"Step: {Step.Pretty(level + 1)}");
            if (IsIndexing)
                sb.AppendLine(Display.Utils.Indent(level + 1) + $"IsIndexing: True");

            return sb.ToString();
        }
    }*/
}