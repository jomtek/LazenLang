using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Loops
{
    class ForLoop : Instr
    {
        public Identifier Id;
        public ExprNode Array;
        public Block Block;

        public ForLoop(Identifier id, ExprNode array, Block block)
        {
            Id = id;
            Array = array;
            Block = block;
        }

        public static ForLoop Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.FOR);
            Identifier id;
            ExprNode array;
            Block block;

            try
            {
                id = parser.TryConsumer(Identifier.Consume);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.IDENTIFIER),
                    parser.Cursor
                );
            }

            parser.Eat(TokenInfo.TokenType.IN, false);

            try
            {
                array = parser.TryConsumer(ExprNode.Consume);
            } catch (ParserError ex)
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
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid instruction in FOR loop"),
                    parser.Cursor
                );
            }

            if (parsedInstr.Value is Block)
                block = (Block)parsedInstr.Value;
            else
                block = new Block(new InstrNode[] { parsedInstr });

            return new ForLoop(id, array, block);
        }

        public override string Pretty()
        {
            return $"ForLoop(id: {Id.Pretty()}, array: {Array.Pretty()}, block: {Block.Pretty()})";
        }
    }
}