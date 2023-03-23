using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using Typechecking.Errors;

namespace Typechecking.Investigation.Investigators
{
    internal static class InfixOpInvestigator
    {
        public static TypeDesc Investigate(InfixOp infixOp, LocalContext context, CodePosition pos)
        {
            var t1 = TypeInvestigator.Investigate(
                new ExprNode(infixOp.LeftOperand, pos), context);
            var t2 = TypeInvestigator.Investigate(
                new ExprNode(infixOp.RightOperand, pos), context);

            switch (infixOp.Operator.Type)
            {
                case TokenInfo.TokenType.EQ:
                case TokenInfo.TokenType.NOT_EQ:
                    // Types of left and right operands should be equal if the objects are compared
                    // TODO : make it more robust, using an IEquatable interface for example
                    if (TypeComparator.Compare(t1, t2))
                    {
                        // Of course, such an op returns a boolean value
                        return new NameType("Bool");
                    }
                    else
                    {
                        var pp1 = TypeDisplayer.Pretty(t1);
                        var pp2 = TypeDisplayer.Pretty(t2);
                        throw new TypeCheckerError(new ComparisonProblem(pp1, pp2), pos);
                    }

                case TokenInfo.TokenType.BOOLEAN_AND:
                case TokenInfo.TokenType.BOOLEAN_OR:
                    // Types of left and right operands should be Bool
                    Utils.MatchTypes(new NameType("Bool"), t1, pos);
                    Utils.MatchTypes(new NameType("Bool"), t2, pos);
                    return new NameType("Bool");

                case TokenInfo.TokenType.GREATER:
                case TokenInfo.TokenType.LESS:
                case TokenInfo.TokenType.PLUS:
                case TokenInfo.TokenType.MINUS:
                case TokenInfo.TokenType.DIVIDE:
                case TokenInfo.TokenType.MULTIPLY:
                case TokenInfo.TokenType.POWER:
                case TokenInfo.TokenType.GREATER_EQ:
                case TokenInfo.TokenType.LESS_EQ:
                    // Types of left and right operands should be Int or Double
                    // TODO : typesystem is inconsistent. make it consistent.
                    Utils.MatchManyTypes(
                        new List<TypeDesc> { new NameType("Int"), new NameType("Double") },
                        t1, pos);
                    Utils.MatchManyTypes(
                        new List<TypeDesc> { new NameType("Int"), new NameType("Double") },
                        t2, pos);
                
                    if (TypeComparator.Compare(t1, new NameType("Double")) ||
                        TypeComparator.Compare(t2, new NameType("Double")))
                    {
                        // If either one operand is a double, the result is a double
                        return new NameType("Double");
                    }
                    else
                    {
                        return new NameType("Int");
                    }
                
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
