using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Expressions.OOP;
using LazenLang.Parsing.Display;
using System;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Ast.Statements
{
    public class VarMutation : Instr, IPrettyPrintable
    {
        public Expr BaseVariable;
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

        public VarMutation(TokenInfo.TokenType mutationOp, Expr baseVariable, Expr newValue)
        {
            BaseVariable = baseVariable;
            MutationOp = mutationOp;
            NewValue = newValue;
        }

        public static VarMutation Consume(Parser parser)
        {
            Expr baseVariable = null;
            TokenInfo.TokenType mutationOp;
            Expr newValue = null;

            baseVariable = parser.TryConsumer(ExprNode.Consume).Value;

            if (!(baseVariable is Identifier) && !(baseVariable is AttributeAccess))
                throw new ParserError(new FailedConsumer(), parser.Cursor);

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

            return new VarMutation(mutationOp, baseVariable, newValue);
        }

        public override string Pretty(int level)
        {
            // TODO: why is base variable not an identifier ?

            var sb = new StringBuilder("Variable Mutation");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Operator: {MutationOp}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Base Variable: {BaseVariable.Pretty(level + 1)}");
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"New Value: {NewValue.Pretty(level + 1)}");     

            return sb.ToString();      
        }
    }
}