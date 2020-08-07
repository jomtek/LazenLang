using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    class BreakInstr : Instr
    {
        public static BreakInstr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.BREAK);
            return new BreakInstr();
        }

        public override string Pretty()
        {
            return "BreakInstr()";
        }
    }
}