using LazenLang.Parsing.Display;
using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    public class FuncDecl : Instr, IPrettyPrintable
    {
        public Signature Signature;
        public Block Block;

        public FuncDecl(Signature signature, Block block)
        {
            Signature = signature;
            Block = block;
        }

        public static FuncDecl Consume(Parser parser, bool inClass = false)
        {
            Signature signature = null;
            Block block = null;

            signature = parser.TryConsumer((Parser p) => Signature.Consume(p, inClass));
            block = parser.TryConsumer((Parser p) => Block.Consume(p));

            return new FuncDecl(signature, block);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("FuncDecl");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"{Signature.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"{Block.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}
