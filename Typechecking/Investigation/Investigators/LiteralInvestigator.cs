using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;

namespace Typechecking.Investigation.Investigators
{
    internal static class LiteralInvestigator
    {
        public static TypeDesc Investigate(Literal literal, LocalContext context, CodePosition pos)
        {
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
                    context.AssertExists(idenLit.Value, pos);
                    return context.GetTypeUnsafe(idenLit.Value);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
