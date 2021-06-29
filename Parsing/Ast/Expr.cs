using LazenLang.Lexing;
using LazenLang.Parsing.Algorithms;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Arrays;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Expressions.OOP;
using LazenLang.Parsing.Display;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazenLang.Parsing.Ast
{
    public abstract class Expr : IPrettyPrintable
    {
        public abstract string Pretty(int level);
    }

    public class ExprNode : IPrettyPrintable
    {
        public Expr Value;
        public CodePosition Position;

        public ExprNode(Expr value, CodePosition position)
        {
            Value = value;
            Position = position;
        }

        private readonly static TokenInfo.TokenType[] _operators = {
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

        private static Expr ParseParenthesisExpr(Parser parser)
        {
            Token leftParenthesis = parser.Eat(TokenInfo.TokenType.L_PAREN);
            Expr expr = parser.TryConsumer(Consume).Value;
            
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
            if (!_operators.Contains(parser.LookAhead().Type))
                throw new ParserError(new FailedConsumer(), parser.Cursor);
            return parser.TryManyEats(_operators);
        }

        private static Expr ParseOperand(Parser parser)
        {
            Expr operand = null;

            switch (parser.LookAhead().Type)
            {
                case TokenInfo.TokenType.IDENTIFIER:
                    operand = parser.TryManyConsumers(new Func<Parser, Expr>[]
                    {
                    FuncCall.Consume,
                    Literal.Consume
                    });
                    break;

                case TokenInfo.TokenType.DOUBLE_LIT:
                case TokenInfo.TokenType.INTEGER_LIT:
                case TokenInfo.TokenType.STRING_LIT:
                case TokenInfo.TokenType.CHAR_LIT:
                case TokenInfo.TokenType.BOOLEAN_LIT:
                    operand = parser.TryConsumer(Literal.Consume);
                    break;

                case TokenInfo.TokenType.L_BRACKET:
                    operand = parser.TryConsumer(NativeArray.Consume);
                    break;

                case TokenInfo.TokenType.NOT:
                    operand = parser.TryConsumer(NotExpr.Consume);
                    break;

                case TokenInfo.TokenType.MINUS:
                case TokenInfo.TokenType.PLUS:
                    operand = parser.TryConsumer(NegExpr.Consume);
                    break;

                case TokenInfo.TokenType.IF:
                    operand = parser.TryConsumer(IfInstr.Consume);
                    break;

                case TokenInfo.TokenType.FUNC:
                    operand = parser.TryConsumer(Lambda.Consume);
                    break;

                case TokenInfo.TokenType.NEW:
                    operand = parser.TryConsumer(Instanciation.Consume);
                    break;

                case TokenInfo.TokenType.THIS:
                    operand = parser.TryConsumer(This.Consume);
                    break;

                case TokenInfo.TokenType.L_PAREN:
                    operand = parser.TryConsumer(ParseParenthesisExpr);
                    break;
            }

            if (operand == null)
                throw new ParserError(new FailedConsumer(), parser.Cursor);

            return operand;
        }

        private static Expr ParseBinOpSeq(Parser parser, bool uniOpPrivilege = false)
        {
            var operands = new List<Expr>();
            var operators = new List<Token>();

            while (true)
            {
                try
                {
                    operands.Add(ParseOperand(parser));
                    if (uniOpPrivilege) break;
                    operators.Add(ParseOperator(parser));
                } catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive())
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

        public string Pretty(int level)
        {
            return "ExprNode: " + Value.Pretty(level);
        }
    }
}