using LazenLang.Parsing.Ast;
using System.Collections.Generic;

namespace LazenLang.Parser.Ast
{
    class Block : Instr
    {
        public List<Instr> instructions { get; }

        public Block(List<Instr> instructions)
        {
            this.instructions = instructions;
        }
    }
}