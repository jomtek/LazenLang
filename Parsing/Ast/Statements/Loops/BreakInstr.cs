using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    public class BreakInstr : Instr, IPrettyPrintable
    {
        public static BreakInstr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.BREAK);
            return new BreakInstr();
        }

        public override string Pretty(int level)
        {
            return "BreakInstr";
        }
    }
}