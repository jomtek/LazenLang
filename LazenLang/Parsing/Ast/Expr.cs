using LazenLang.Lexing;
using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Ast
{

    abstract class Expr { }
    /*struct ExprNode
    {
        public Expr Expression;
        public CodePosition Position;

        public ExprNode(Expr expression, CodePosition position)
        {
            Expression = expression;
            Position = position;
        }
    }*/

    class ExprNode
    {
        public Expr Value;
        public CodePosition Position;

        private static TokenInfo.TokenType[] operators = {
            TokenInfo.TokenType.EQ,
            TokenInfo.TokenType.NOT_EQ,
            TokenInfo.TokenType.BOOLEAN_AND,
            TokenInfo.TokenType.BOOLEAN_OR,
            TokenInfo.TokenType.IN,
            TokenInfo.TokenType.GREATER,
            TokenInfo.TokenType.LESS,
            TokenInfo.TokenType.PLUS,
            TokenInfo.TokenType.MINUS,
            TokenInfo.TokenType.DIVIDE,
            TokenInfo.TokenType.MULTIPLY,
            TokenInfo.TokenType.POWER,
            TokenInfo.TokenType.DOT,
            TokenInfo.TokenType.MODULO,
            TokenInfo.TokenType.GREATER_EQ,
            TokenInfo.TokenType.LESS_EQ
        };

        public ExprNode(Expr value, CodePosition position)
        {
            Value = value;
            Position = position;
        }

        private static Token ParseOperator(Parser parser)
        {
            return parser.TryManyEats(operators);
        }

        private static ExprNode ParseOperand(Parser parser)
        {
            return parser.TryManyConsumers(
                new Func<Parser, ExprNode>[]{
                    Literal.Consume
                }, 
                parser
            );
        }

        public static Expr ParseBinOpSeq(Parser parser, bool uniOpPrivilege = false)
        {
            var operands = new List<ExprNode>();
            var operators = new List<Token>();
            CodePosition oldCursor = parser.cursor;

            while (true)
            {
                try
                {
                    operands.Add(ParseOperand(parser));
                    if (uniOpPrivilege) break;
                    operators.Add(ParseOperator(parser));
                } catch (ParserError ex)
                {
                    if (!ex.IsErrorFromParserClass())
                        throw ex;
                    else
                        break;
                }
            }
      
            if (operands.Count == 0)
            {
                throw new ParserError(
                    new FailedConsumer(), parser.cursor
                );
            }
            else if (operators.Count > operands.Count - 1)
            {
                throw new ParserError(
                    new UnexpectedTokenException(operators.Last().Type),
                    operators.Last().Pos
                );
            }
            else if (operands.Count == 1 && uniOpPrivilege)
            {
                return operands[0].Value;
            }

            foreach (ExprNode operand in operands)
            {
                Console.WriteLine(operand.Value.ToString());
            }

            throw new NotImplementedException();
        }

        public static ExprNode Consume()
        {

            throw new NotImplementedException();
        }
    }
}
