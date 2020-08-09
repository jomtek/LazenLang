using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements
{
    class VarMutation : Instr
    {
        public Identifier BaseVariable;
        public TokenInfo.TokenType MutationOp;
        public Expr NewValue;

        private static readonly TokenInfo.TokenType[] mutationOperators = new TokenInfo.TokenType[]
        {
            TokenInfo.TokenType.ASSIGN,
            TokenInfo.TokenType.MINUS_EQ,
            TokenInfo.TokenType.PLUS_EQ,
            TokenInfo.TokenType.MULTIPLY_EQ,
            TokenInfo.TokenType.DIVIDE_EQ,
            TokenInfo.TokenType.POWER_EQ,
            TokenInfo.TokenType.MODULO_EQ
        };

        public VarMutation(Identifier baseVariable, TokenInfo.TokenType mutationOp, Expr newValue)
        {
            BaseVariable = baseVariable;
            MutationOp = mutationOp;
            NewValue = newValue;
        }

        public static VarMutation Consume(Parser parser)
        {
            Identifier baseVariable = null;
            TokenInfo.TokenType mutationOp;
            Expr newValue = null;

            baseVariable = parser.TryConsumer(Identifier.Consume);

            if (mutationOperators.Contains(parser.LookAhead().Type))
                mutationOp = parser.Eat(parser.LookAhead().Type).Type;
            else
                throw new ParserError(new FailedConsumer(), parser.Cursor);

            try
            {
                newValue = parser.TryConsumer(ExprNode.Consume).Value;
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new ExpectedElementException("Expected expression after mutation operator"),
                    parser.Cursor
                );
            }

            return new VarMutation(baseVariable, mutationOp, newValue);
        }

        public override string Pretty()
        {
            return $"VarMutation(basevar: {BaseVariable.Pretty()}, op: {MutationOp}, newvalue: {NewValue.Pretty()})";
        }
    }
}
