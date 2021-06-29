using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions
{
    public class InfixOp : Expr, IPrettyPrintable
    {
        public Expr LeftOperand { get; }
        public Expr RightOperand { get; }
        public Token Operator { get; }

        public InfixOp(Expr leftOperand, Expr rightOperand, Token op)
        {
            LeftOperand = leftOperand;
            RightOperand = rightOperand;
            Operator = op;
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("InfixOp");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Operator: {Operator.Type}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Left: {LeftOperand.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Right: {RightOperand.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}
