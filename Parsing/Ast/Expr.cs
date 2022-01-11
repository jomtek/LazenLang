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
using static LazenLang.Lexing.TokenInfo;

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
            Expr expr = parser.TryConsumer((p) => Consume(p)).Value;
            
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

        private static Expr ParseOperand(Parser parser, bool excludeSlicingAndIndexing)
        {
            Expr operand = null;

            switch (parser.LookAhead().Type)
            {
                case TokenInfo.TokenType.IDENTIFIER:
                    operand = parser.TryManyConsumers(new (bool, Func<Parser, Expr>)[]
                    {
                        (true, FuncCall.Consume),
                        //(!excludeSlicingAndIndexing, ArrayIndexing.Consume),
                        //(!excludeSlicingAndIndexing, ArraySlicing.Consume),
                        (true, Literal.Consume),
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


        private static Expr ParseBinOpSeq(Parser parser, bool excludeSlicingAndIndexing)
        {
            var operands = new List<Expr>();
            var operators = new List<Token>();

            while (true)
            {
                try
                {
                    Expr operand = ParseOperand(parser, excludeSlicingAndIndexing);

                    // Manage indexing (i.e. a[b][c]) and slicing (i.e. a[c:d]) using continuous folding
                    ArraySlicing slicing = null;
                    while (parser.LookAhead().Type == TokenType.L_BRACKET)
                    {
                        ExprNode beginning = null;
                        ExprNode ending = null;
                        ExprNode step = null;
                        bool isIndexing = true;

                        parser.Eat(TokenType.L_BRACKET);

                        // list[  a <-here  :c:d]
                        if (parser.LookAhead().Type != TokenType.COLON)
                            beginning = parser.TryConsumer(ExprNode.Consume);
                        
                        if (parser.LookAhead().Type == TokenType.COLON)
                        {
                            isIndexing = false;

                            // list[a  : <-here  b:c]
                            parser.Eat(TokenType.COLON);

                            // list[a:  b <-here  :c]
                            if (parser.LookAhead().Type != TokenType.R_BRACKET &&
                                parser.LookAhead().Type != TokenType.COLON)
                                ending = parser.TryConsumer(ExprNode.Consume);

                            // list[a:b  : <-here  c]
                            if (parser.LookAhead().Type == TokenType.COLON)
                            {
                                parser.Eat(TokenType.COLON);

                                // list[a:b:  c <-here  ]
                                if (parser.LookAhead().Type != TokenType.R_BRACKET)
                                {
                                    step = parser.TryConsumer(ExprNode.Consume);
                                }
                            }
                        }

                        slicing = new ArraySlicing(slicing == null ? operand : slicing, beginning, ending, step, isIndexing);
                        parser.Eat(TokenType.R_BRACKET, false);
                    }

                    operands.Add(slicing == null ? operand : slicing);

                    operators.Add(ParseOperator(parser));
                }
                catch (ParserError ex)
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
            return Consume(parser, false);
        }

        public static ExprNode Consume(Parser parser, bool excludeSlicingAndIndexing)
        {
            CodePosition oldCursor = parser.Cursor;
            return new ExprNode(
                parser.TryConsumer((Parser p) => ParseBinOpSeq(p, excludeSlicingAndIndexing)),
                oldCursor
            );
        }

        public string Pretty(int level)
        {
            return "ExprNode: " + Value.Pretty(level);
        }
    }
}