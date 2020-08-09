using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types
{
    class NameType : Type
    {
        public Identifier Name { get; }

        public NameType(Identifier name)
        {
            Name = name;
        }

        public static NameType Consume(Parser parser)
        {
            Identifier name = parser.TryConsumer(Identifier.Consume);
            return new NameType(name);
        }

        public override string Pretty()
        {
            return $"NameType(`{Name.Value}`)";
        }
    }
}
