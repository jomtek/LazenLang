using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    public class WhileLoop : Instr, IPrettyPrintable
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
            } 
            catch (ParserError ex)
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
            }
            catch (ParserError ex)
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

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("WhileLoopInstr");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Condition: {Condition.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Block: {Block.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}