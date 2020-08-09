using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements
{
    class ExprInstr : Instr
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

        public override string Pretty()
        {
            return $"ExprInstr({Expression.Pretty()})";
        }
    }
}