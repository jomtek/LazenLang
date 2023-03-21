using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using Parsing.Ast;

namespace LazenLang.Parsing.Ast.Types
{
    public class NameType : TypeDesc, IPrettyPrintable
    {
        public String Name;

        public NameType(String name)
        {
            Name = name;
        }

        public static NameType Consume(Parser parser)
        {
            Identifier name = parser.TryConsumer(Identifier.Consume);
            return new NameType(name.Value);
        }

        public override string Pretty(int level)
        {
            return "NameType: " + Name;
        }
    }
}
