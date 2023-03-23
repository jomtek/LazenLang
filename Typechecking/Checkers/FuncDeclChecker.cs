using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Typechecking.Checkers
{
    internal class FuncDeclChecker
    {
        public static void Check(FuncDecl funcDecl, LocalContext context, CodePosition pos)
        {
            // Generate a FuncType
            var domain = new List<TypeDesc>();
            foreach (Param param in funcDecl.Signature.Domain)
                domain.Add(param.Type.Value);
            var codomain = funcDecl.Signature.Codomain.Value;
            var ftype = new FuncType(domain.ToArray(), codomain);

            // Add function to the local context
            // TODO : allow function overloading?
            string fname = funcDecl.Signature.Name.Value;
            context.Add(fname, ftype, pos);

            // Generate a subcontext for the function
            var subContext = LocalContext.CopyFrom(context);
            subContext.CurrentCodomain = codomain;

            // Add the parameters
            foreach (Param param in funcDecl.Signature.Domain)
            {
                /*
                Here, shadowing should be allowed
                baz: Bool = True
                func foo(baz: Int) {} // This is okay, baz is overriden in the context
                */
                subContext.Add(param.Name.Value, param.Type.Value, param.Type.Position, true);
            }

            // Typecheck the block
            // No side effects on the parent context
            new BlockTypeChecker(funcDecl.Block, subContext).Check();
        }
    }
}
