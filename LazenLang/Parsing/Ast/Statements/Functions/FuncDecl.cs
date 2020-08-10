using System;
using System.Collections.Generic;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    class FuncDecl : Instr
    {
        public Signature Signature;
        public Block Block;

        public FuncDecl(Signature signature, Block block)
        {
            Signature = signature;
            Block = block;
        }

        public static FuncDecl Consume(Parser parser)
        {
            Signature signature = null;
            Block block = null;

            signature = parser.TryConsumer(Signature.Consume);

            InstrNode parsedInstr;
            try
            {
                parsedInstr = parser.TryConsumer(InstrNode.Consume);
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected block or instruction after signature for function declaration"),
                    parser.Cursor
                );
            }

            block = Utils.InstrToBlock(parsedInstr);
            return new FuncDecl(signature, block);
        }

        public override string Pretty()
        {
            return $"FuncDecl(signature: {Signature.Pretty()}, block: {Block.Pretty()})";
        }
    }
}
