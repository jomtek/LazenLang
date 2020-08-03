using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Algorithms
{
    class ShuntingYard
    {
        public static InfixOp Go(List<Expr> operands, List<Token> operators)
        {
            foreach (Expr operand in operands)
                Console.WriteLine("operand : " + operand.ToString());
            foreach (Token op in operators)
                Console.WriteLine("operator : " + op.Value);
            throw new NotImplementedException();
        }
    }
}