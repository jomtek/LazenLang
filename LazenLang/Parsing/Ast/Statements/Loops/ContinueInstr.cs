using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    class ContinueInstr : Instr
    {
        public static ContinueInstr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.CONTINUE);
            return new ContinueInstr();
        }

        public override string Pretty()
        {
            return "ContinueInstr()";
        }
    }
}