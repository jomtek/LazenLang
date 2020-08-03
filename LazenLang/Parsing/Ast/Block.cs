using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using System.Collections.Generic;

namespace LazenLang.Parsing.Ast
{
    class Block
    {
        public List<Instr> instructions { get; }

        public Block(List<Instr> instructions)
        {
            this.instructions = instructions;
        }

        public static Block Consume(Parser parser)
        {
            var instructions = new List<Instr>();

            parser.Eat(TokenInfo.TokenType.L_CURLY_BRACKET);
            // TODO
            parser.Eat(TokenInfo.TokenType.R_CURLY_BRACKET);

            return new Block(instructions);
        }
    }
}