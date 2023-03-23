using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Typechecking
{
    internal static class TypeDisplayer
    {
        // This class displays TypeDesc instances in a natural, readable way
        public static string Pretty(TypeDesc typeDesc)
        {
            switch (typeDesc)
            {
                case NameType name:
                    return name.Name;

                case FuncType funct:
                    var sb = new StringBuilder("Func(");
                    var prettyDomain = new List<String>();
                    foreach (var param in funct.Domain)
                        prettyDomain.Add(Pretty(param));
                    sb.Append(string.Join(", ", prettyDomain));
                    sb.Append(") -> ");
                    sb.Append(Pretty(funct.Codomain));
                    return sb.ToString();

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
