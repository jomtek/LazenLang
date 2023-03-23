using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Literals;
using Parsing.Ast;
using System;
using Typechecking.Investigation.Investigators;

namespace Typechecking.Investigation
{
    internal static class TypeInvestigator
    {
        // The following method, as its name tells, tries to guess the type of an Expr object.
        // Meanwhile, it applies some basic typechecking rules when possible.
        public static TypeDesc Investigate(ExprNode expr, LocalContext context)
        {
            switch (expr.Value)
            {
                case Literal literal:
                    return LiteralInvestigator.Investigate(literal, context, expr.Position);
                
                case InfixOp infixOp:
                    return InfixOpInvestigator.Investigate(infixOp, context, expr.Position);

                case FuncCall funcCall:
                    return FuncCallInvestigator.Investigate(funcCall, context, expr.Position);

                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
