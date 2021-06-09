using LazenLang.Parsing.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements
{
    public class ExprInstr : Instr, IPrettyPrintable
    {
        public Expr Expression { get; }

        public ExprInstr(Expr expression)
        {
            Expression = expression;
        }

        public static ExprInstr Consume(Parser parser)
        {
            Expr expression = parser.TryConsumer(ExprNode.Consume).Value;
            return new ExprInstr(expression);
        }

        public override string Pretty(int level)
        {
            return $"ExprInstr: {Expression.Pretty(level)}";
        }
    }
}