using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;

namespace Typechecking
{
    public static class TypeInvestigator
    {
        // The type investigator, as its name tells, tries to guess the type of an Expr object.
        // Meanwhile, it applies some basic typechecking rules when possible.
        public static TypeDesc Investigate(ExprNode expr, LocalContext context)
        {
            switch (expr.Value)
            {
                case Literal literal:
                    switch (literal)
                    {
                        case BooleanLit booleanLit:
                            return new NameType("Bool");
                        case CharLit charLit:
                            return new NameType("Char");
                        case DoubleLit doubleLit:
                            return new NameType("Double");
                        case IntegerLit integerLit:
                            return new NameType("Int");
                        case StringLit stringLit:
                            return new NameType("String");
                        case Identifier idenLit:
                            context.AssertExists(idenLit.Value, expr.Position);
                            return context.GetTypeUnsafe(idenLit.Value);
                    }
                    break;

                default:
                    break;
            }

            throw new NotSupportedException();
        }
    }
}
