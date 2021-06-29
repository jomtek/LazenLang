using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    public class ContinueInstr : Instr, IPrettyPrintable
    {
        public static ContinueInstr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.CONTINUE);
            return new ContinueInstr();
        }

        public override string Pretty(int level)
        {
            return "ContinueInstr";
        }
    }
}