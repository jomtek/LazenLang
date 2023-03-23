using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Statements.Functions;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Typechecking.Errors;
using Typechecking.Investigation;

namespace Typechecking.Checkers
{
    internal class ReturnInstrChecker
    {
        public static void Check(ReturnInstr returnInstr, LocalContext context, CodePosition pos)
        {
            if (context.CurrentCodomain == null)
            {
                throw new TypeCheckerError(
                    new MiscProblem("Unexpected RETURN instruction outside function"),
                    pos);
            }

            // Check the return type
            TypeDesc guessedType = TypeInvestigator.Investigate(returnInstr.Value, context);
            Utils.MatchTypes(context.CurrentCodomain, guessedType, pos);
        }
    }
}
