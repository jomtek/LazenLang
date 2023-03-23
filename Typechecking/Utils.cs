using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using Parsing.Ast;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Xml.XPath;
using Typechecking.Errors;
using Typechecking.Investigation;

namespace Typechecking
{
    internal static class Utils
    {
        public static void CheckExpr(TypeDesc expected, ExprNode candidate, LocalContext context)
        {
            // First off, guess the type of the candidate expression
            TypeDesc guessedType = TypeInvestigator.Investigate(candidate, context);

            // Compare it with the expected type
            if (!TypeComparator.Compare(expected, guessedType))
            {
                var expectedType = TypeDisplayer.Pretty(expected);
                var gotType = TypeDisplayer.Pretty(guessedType);
                throw new TypeCheckerError(new TypesMismatched(expectedType, gotType), candidate.Position);
            }
        }

        public static void MatchTypes(TypeDesc expected, TypeDesc got, CodePosition pos)
        {
            if (!TypeComparator.Compare(expected, got))
            {
                var expectedType = TypeDisplayer.Pretty(expected);
                var gotType = TypeDisplayer.Pretty(got);
                throw new TypeCheckerError(new TypesMismatched(expectedType, gotType), pos);
            }
        }

        public static void MatchManyTypes(List<TypeDesc> accepted, TypeDesc got, CodePosition pos)
        {
            foreach (var type in accepted)
                if (TypeComparator.Compare(type, got))
                    return;

            var prettyAccepted = new List<string>();
            foreach (var type in accepted)
                prettyAccepted.Add(TypeDisplayer.Pretty(type));

            var gotType = TypeDisplayer.Pretty(got);
            throw new TypeCheckerError(new MultiTypesMismatched(prettyAccepted, gotType), pos);
        }
    }
}
