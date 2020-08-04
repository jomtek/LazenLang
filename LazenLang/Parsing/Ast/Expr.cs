using LazenLang.Lexing;
using LazenLang.Parsing.Algorithms;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Literals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Ast
{

    abstract class Expr
    {
        public virtual string Pretty()
        {
            return "Expr";
        }
    }

    class ExprNode
    {
        public Expr Value;
        public CodePosition Position;

        private readonly static TokenInfo.TokenType[] operators = {
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

        private static ExprNode ParseParenthesisExpr(Parser parser)
        {
            Token leftParenthesis = parser.Eat(TokenInfo.TokenType.L_PAREN);
            ExprNode expr = parser.TryConsumer(Consume);
            
            try
            {
                parser.Eat(TokenInfo.TokenType.R_PAREN);
            } catch (ParserError)
            {
                throw new ParserError(
                    new ExpectedTokenException(TokenInfo.TokenType.R_PAREN),
                    parser.Cursor
                );
            }

            return expr;
        }

        private static Token ParseOperator(Parser parser)
        {
            return parser.TryManyEats(operators);
        }

        private static ExprNode ParseOperand(Parser parser)
        {
            return parser.TryManyConsumers(
                new Func<Parser, ExprNode>[]{
                    Literal.Consume,
                    NegExpr.Consume,
                    NegNum.Consume,
                    ParseParenthesisExpr
                }
            );
        }

        private static Expr ParseBinOpSeq(Parser parser, bool uniOpPrivilege = false)
        {
            var operands = new List<Expr>();
            var operators = new List<Token>();

            while (true)
            {
                try
                {
                    operands.Add(ParseOperand(parser).Value);
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
                    new FailedConsumer(), parser.Cursor
                );
            }
            else if (operators.Count > operands.Count - 1)
            {
                throw new ParserError(
                    new UnexpectedTokenException(operators.Last().Type),
                    operators.Last().Pos
                );
            }
            else if (operands.Count == 1)
            {
                return operands[0];
            }

            return ShuntingYard.Go(operands, operators);
        }

        public static ExprNode Consume(Parser parser)
        {
            CodePosition oldCursor = parser.Cursor;
            return new ExprNode(
                parser.TryConsumer((Parser p) => ParseBinOpSeq(p)),
                oldCursor
            );
        }

        public string Pretty()
        {
            return $"ExprNode(value: {Value.Pretty()}, pos: {Position.Pretty()})";
        }
    }
}