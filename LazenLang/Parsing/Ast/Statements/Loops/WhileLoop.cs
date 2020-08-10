using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    class WhileLoop : Instr
    {
        public ExprNode Condition;
        public Block Block;

        public WhileLoop(ExprNode condition, Block block)
        {
            Condition = condition;
            Block = block;
        }

        public static WhileLoop Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.WHILE);
            ExprNode condition;
            Block resultBlock;

            try
            {
                condition = parser.TryConsumer(ExprNode.Consume);
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid condition after WHILE token"),
                    parser.Cursor
                );
            }

            InstrNode parsedInstr;
            try
            {
                parsedInstr = parser.TryConsumer(InstrNode.Consume);
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid instruction or block for WHILE instruction"),
                    parser.Cursor
                );
            }

            resultBlock = Utils.InstrToBlock(parsedInstr);
            return new WhileLoop(condition, resultBlock);
        }

        public override string Pretty()
        {
            return $"WhileInstr(condition: {Condition.Value.Pretty()}, block: {Block.Pretty()})";
        }
    }
}