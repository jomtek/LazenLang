using LazenLang.Parsing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Expressions.Literals;
using LazenLang.Parsing.Ast.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LazenLang.Typechecking.Tools
{
    class Utils
    {
        public static int typevarNum = 0;
        public static TypeVariable FreshTypeVariable()
        {
            typevarNum++;
            return new TypeVariable(typevarNum);
        }

        public static ExprNode GetBlockLast(Block block)
        {
            if (block.Instructions.Count() > 0)
            {
                InstrNode last = block.Instructions.Last();
                if (last.Value is ExprInstr)
                {
                    return new ExprNode(((ExprInstr)last.Value).Expression, last.Position);
                } else
                {
                    throw new ParserError(
                        new InvalidElementException("Invalid last instruction for block"),
                        last.Position
                    );
                }
            } else
            {
               throw new ArgumentException();
            }
        }
    }
}
