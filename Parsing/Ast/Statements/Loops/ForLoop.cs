using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using LazenLang.Parsing.Ast.Statements.Functions;
using Parsing.Ast;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    public class ForLoop : Instr, IPrettyPrintable, ICreatesSingleBlock
    {
        public Param Id;
        public ExprNode Array;
        public Block Block { get; set; }

        public ForLoop(Param id, ExprNode array, Block block)
        {
            Id = id;
            Array = array;
            Block = block;
        }

        public static ForLoop Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FOR);
            Param id;
            ExprNode array;
            Block block;

            try
            {
                id = parser.TryConsumer(Param.Consume);
            }
            catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedElementException("Expected typed identifier, such as `i: Int`"),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.IN, false);

            try
            {
                array = parser.TryConsumer(ExprNode.Consume);
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected expression after IN token"),
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
                    new InvalidElementException("Invalid instruction in FOR loop"),
                    parser.Cursor
                );
            }

            block = Utils.InstrToBlock(parsedInstr);
            return new ForLoop(id, array, block);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("ForLoopInstr");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Id: {Id.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Iterate over: {Array.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Block: {Block.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}