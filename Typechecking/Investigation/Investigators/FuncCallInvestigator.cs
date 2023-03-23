using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Typechecking.Errors;

namespace Typechecking.Investigation.Investigators
{
    internal static class FuncCallInvestigator
    {
        public static TypeDesc Investigate(FuncCall funcCall, LocalContext context, CodePosition pos)
        {
            context.AssertExists(funcCall.Name.Value, pos);
            var ftypeUnsafe = context.GetTypeUnsafe(funcCall.Name.Value);
            if (!(ftypeUnsafe is FuncType))
            {
                // Calling a non-function
                // TODO : implement the class instantiation syntax
                // Example : c:MyClass = MyClass();
                var display = TypeDisplayer.Pretty(ftypeUnsafe);
                throw new TypeCheckerError(
                    new MiscProblem($"Trying to call a non-callable `{display}` object"),
                    pos);
            }

            var ftype = (FuncType)ftypeUnsafe;
            
            if (funcCall.Arguments.Length != ftype.Domain.Length)
            {
                throw new TypeCheckerError(
                    new MiscProblem($"Call does not match signature ({funcCall.Arguments.Length} args instead of {ftype.Domain.Length})"),
                    pos);
            }

            for (int i = 0; i < funcCall.Arguments.Length; i++)
            {
                // Compare argument type with signature positional parameter type
                ExprNode argument = funcCall.Arguments[i];
                TypeDesc domainPositionalType = ftype.Domain[i];
                var guessedType = TypeInvestigator.Investigate(argument, context);
                Utils.MatchTypes(domainPositionalType, guessedType, argument.Position);
            }

            return ftype.Codomain;
        }
    }
}
