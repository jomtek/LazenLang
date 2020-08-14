using LazenLang.Lexing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.OOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace LazenLang.Parsing.Algorithms
{
    class ShuntingYard
    {
        private static void FoldLastOperands(ref List<Expr> operands, Token op)
        {
            Expr leftOperand = operands[operands.Count - 2];
            Expr rightOperand = operands[operands.Count - 1];

            Expr operation;         
            if (op.Type == TokenInfo.TokenType.DOT)
                operation = new AttributeAccess(leftOperand, rightOperand);
            else
                operation = new InfixOp(leftOperand, rightOperand, op);

            operands.RemoveAt(operands.Count - 1);
            operands.RemoveAt(operands.Count - 1);

            operands.Add(operation);
        }

        public static Expr Go(List<Expr> operands, List<Token> operators)
        {
            var operatorPrecedences = new Dictionary<TokenInfo.TokenType, int>()
            {
                [TokenInfo.TokenType.DOT] = 7,
                [TokenInfo.TokenType.POWER] = 6,
                [TokenInfo.TokenType.DIVIDE] = 5,
                [TokenInfo.TokenType.MULTIPLY] = 5,
                [TokenInfo.TokenType.MODULO] = 5,
                [TokenInfo.TokenType.PLUS] = 4,
                [TokenInfo.TokenType.MINUS] = 4,
                [TokenInfo.TokenType.GREATER] = 3,
                [TokenInfo.TokenType.LESS] = 3,
                [TokenInfo.TokenType.GREATER_EQ] = 3,
                [TokenInfo.TokenType.LESS_EQ] = 3,
                [TokenInfo.TokenType.EQ] = 2,
                [TokenInfo.TokenType.NOT_EQ] = 2,
                [TokenInfo.TokenType.IN] = 2,
                [TokenInfo.TokenType.BOOLEAN_AND] = 1,
                [TokenInfo.TokenType.BOOLEAN_OR] = 1
            };

            var operandStack = new List<Expr>();
            var opStack = new List<Token>();

            int operatorIndex = 0;
            int operandIndex = 0;

            bool wasLastOperand = false;
            while (true)
            {
                if (!wasLastOperand)
                {
                    if (operandIndex > operands.Count - 1)
                        break;

                    Expr operand = operands[operandIndex];
                    operandStack.Add(operand);
                    operandIndex++;
                    wasLastOperand = true;
                }
                else
                {
                    if (operatorIndex > operators.Count - 1)
                        break;

                    Token currentOp = operators[operatorIndex];

                    if (opStack.Count > 0)
                    {
                        var stackCopy = new Token[opStack.Count];
                        opStack.CopyTo(stackCopy);

                        foreach (Token op in stackCopy.Reverse())
                        {
                            if (operatorPrecedences[op.Type] >= operatorPrecedences[currentOp.Type])
                            {
                                FoldLastOperands(ref operandStack, op);
                                opStack.RemoveAt(opStack.Count - 1);
                            }
                        }
                    }

                    opStack.Add(currentOp);
                    operatorIndex++;
                    wasLastOperand = false;
                }
            }

            opStack.Reverse();
            foreach (Token op in opStack)
            {
                FoldLastOperands(ref operandStack, op);
            }

            return operandStack[0];
        }
    }
}