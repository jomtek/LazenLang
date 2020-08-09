using System;
using System.Collections.Generic;
using System.Text;
using LazenLang.Lexing;

namespace LazenLang.Parsing.Ast.Statements.Functions
{
    class ReturnInstr : Instr
    {
        public ExprNode Value;

        public ReturnInstr(ExprNode value)
        {
            Value = value;
        }

        public static ReturnInstr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.RETURN);
            
            ExprNode value;
            try
            {
                value = parser.TryConsumer(ExprNode.Consume);
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                value = null;
            }

            return new ReturnInstr(value);
        }

        public override string Pretty()
        {
            string prettyValue = "none";
            if (Value != null) prettyValue = Value.Value.Pretty();
            return $"ReturnInstr({prettyValue})";
        }
    }
}