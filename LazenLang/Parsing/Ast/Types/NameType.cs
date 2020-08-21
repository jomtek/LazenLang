using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Typechecking;
using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Types
{
    class NameTypeC
    {
        public static NameType Consume(Parser parser)
        {
            Identifier name = parser.TryConsumer(Identifier.Consume);
            return new NameType(name);
        }
    }
}
