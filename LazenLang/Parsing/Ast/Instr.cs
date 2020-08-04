using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Statements;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast
{
    abstract class Instr
    {
        public virtual string Pretty()
        {
            return "Instr";
        }
    }

    class InstrNode
    {
        public Instr Value;
        public CodePosition Position;

        public InstrNode(Instr instruction, CodePosition position)
        {
            Value = instruction;
            Position = position;
        }

        public static Instr Consume(Parser parser)
        {
            return parser.TryManyConsumers(new Func<Parser, Instr>[]
            {
                ExprInstr.Consume
            });
        }
    }
}
