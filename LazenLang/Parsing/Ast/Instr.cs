using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Loops;
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

        public static string PrettyMultiple(InstrNode[] instructions)
        {
            string result = "{";
            for (int i = 0; i < instructions.Length; i++)
            {
                Instr instr = instructions[i].Value;
                result += instr.Pretty();
                if (i < instructions.Length - 1) result += ", ";
            }
            return result + "}";
        }

        public static InstrNode Consume(Parser parser)
        {
            CodePosition oldCursor = parser.Cursor;

            Instr instr =  parser.TryManyConsumers(new Func<Parser, Instr>[]
            {
                (Parser p) => Block.Consume(p),
                WhileLoop.Consume,
                ForLoop.Consume,
                BreakInstr.Consume,
                ContinueInstr.Consume,
                ExprInstr.Consume
            });

            return new InstrNode(instr, oldCursor);
        }
    }
}