using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Expressions
{
    class InfixOp : Expr
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
    }
}
