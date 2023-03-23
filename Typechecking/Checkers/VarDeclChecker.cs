using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Statements;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Typechecking.Checkers
{
    internal static class VarDeclChecker
    {
        public static void Check(VarDecl varDecl, LocalContext context, CodePosition pos)
        {
            string vname = varDecl.Name.Value;

            if (varDecl.Type == null)
            {
                // Update value
                // Example: baz = True
                context.AssertExists(vname, pos);
                TypeDesc contextType = context.GetTypeUnsafe(vname);

                /*
                Possible mistake
                baz: Int = 5;
                baz = True;  <--- we're here
                */
                Utils.CheckExpr(contextType, varDecl.Value, context);
            }
            else
            {
                // Make sure the variable value is coherent with its explicit type
                Utils.CheckExpr(varDecl.Type.Value, varDecl.Value, context);

                context.Add(vname, varDecl.Type.Value, pos);
            }
        }
    }
}
