using LazenLang.Lexing;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Types.AtomTypes
{
    class AtomVoid : AtomType
    {
        public new static AtomVoid Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.VOID);
            return new AtomVoid();
        }

        public override string Pretty()
        {
            return "AtomVoid()";
        }
    }
}
