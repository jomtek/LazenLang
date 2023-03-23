using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Text;

namespace Typechecking
{
    internal static class TypeComparator
    {
        // This class compares two TypeDesc instances together
        public static bool Compare(TypeDesc a, TypeDesc b)
        {
            if (a.GetType() != b.GetType())
            {
                return false;
            }

            // More specific comparison...
            switch (a)
            {
                case NameType nameA:
                    var nameB = (NameType)b; // Forced casting is safe
                    return nameA.Name == nameB.Name;

                case FuncType funcA:
                    var funcB = (FuncType)b;
                
                    if (funcA.Domain.Length != funcB.Domain.Length)
                        return false;
                
                    // Domain check
                    for (int i = 0; i < funcA.Domain.Length; i++)
                    {
                        var da = funcA.Domain[i];
                        var db = funcB.Domain[i];
                        if (!Compare(da, db))
                        {
                            return false;
                        }
                    }

                    // Codomain check
                    return Compare(funcA.Codomain, funcB.Codomain);
                
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
