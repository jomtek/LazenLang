using System;
using LazenLang.Lexing;
using System.Collections.Generic;
using System.Text;
using LazenLang.Parsing.Ast.Statements;
using System.Linq;
using LazenLang.Parsing.Display;
using Parsing.Errors;

namespace LazenLang.Parsing.Ast.Expressions
{
    public class IfInstr : Expr, IPrettyPrintable
    {
        public ExprNode Condition;
        public Block MainBranch;
        public Block ElseBranch;

        public IfInstr(ExprNode condition, Block mainBranch, Block elseBranch)
        {
            Condition = condition;
            MainBranch = mainBranch;
            ElseBranch = elseBranch;
        }

        public static IfInstr Consume(Parser parser)
        {
            parser.Eat(TokenInfo.TokenType.IF);

            Block mainBranch = null;
            Block elseBranch = null;
            var elifBranches = new List<IfInstr>();

            #region base_condition
            ExprNode baseCondition;
            try
            {
                baseCondition = parser.TryConsumer(ExprNode.Consume);
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid expression after IF token"),
                    parser.Cursor
                );
            }
            #endregion

            #region main_branch
            InstrNode mainInstr;
            try
            {
                mainInstr = parser.TryConsumer(InstrNode.Consume);
            }
            catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid block in IF instruction"),
                    parser.Cursor
                );
            }

            mainBranch = Utils.InstrToBlock(mainInstr);

            #endregion

            #region elif_branches_collect
            while (true)
            {
                ExprNode condition;
                Block branch;
                
                while (true)
                {
                    try
                    {
                        parser.Eat(TokenInfo.TokenType.EOL);
                    }
                    catch (ParserError)
                    {
                        break;
                    }
                }

                try
                {
                    parser.Eat(TokenInfo.TokenType.ELIF);
                }
                catch (ParserError)
                {
                    break;
                }

                try
                {
                    condition = parser.TryConsumer(ExprNode.Consume);
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new InvalidElementException("Invalid expression after ELIF token"),
                        parser.Cursor
                    );
                }

                try
                {
                    // For each ELIF branch, we want a clear block instead of an instruction
                    branch = parser.TryConsumer((Parser p) => Block.Consume(p));
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new InvalidElementException("Invalid block in ELIF instruction"),
                        parser.Cursor
                    );
                }

                elifBranches.Add(new IfInstr(condition, branch, null));
            }

            #endregion

            #region else_branch
            while (true)
            {
                try
                {
                    parser.Eat(TokenInfo.TokenType.EOL);
                }
                catch (ParserError)
                {
                    break;
                }
            }

            bool isThereElse = true;
            try
            {
                parser.Eat(TokenInfo.TokenType.ELSE);
            }
            catch (ParserError)
            {
                isThereElse = false;
            }

            if (isThereElse)
            {
                InstrNode elseInstr;

                try
                {
                    elseInstr = parser.TryConsumer(InstrNode.Consume);
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new InvalidElementException("Invalid block in ELSE instruction"),
                        parser.Cursor
                    );
                }

                elseBranch = Utils.InstrToBlock(elseInstr);

                if (elifBranches.Count > 0)
                    elifBranches[^1].ElseBranch = elseBranch;
            }
            #endregion

            #region elif_else_fold
            if (elifBranches.Count > 0)
            {
                elifBranches.Reverse();
                for (int i = 0; i < elifBranches.Count - 1; i++)
                {
                    IfInstr branch = elifBranches[i];
                    elifBranches[i + 1].ElseBranch = new Block(new InstrNode[]
                    {
                        new InstrNode(new ExprInstr(branch), new CodePosition())
                    });
                }
            }

            if (elifBranches.Count > 0)
            {
                IfInstr lastElifBranch = elifBranches[^1];
                elseBranch = new Block(new InstrNode[] {
                    new InstrNode(new ExprInstr(lastElifBranch), new CodePosition())
                });
            }
            #endregion

            return new IfInstr(baseCondition, mainBranch, elseBranch);
        }

        public override string Pretty(int level)
        {
            var sb = new StringBuilder("IfInstr");
            sb.AppendLine();
            sb.AppendLine(Display.Utils.Indent(level + 1) + $"Condition: {Condition.Pretty(level + 1)}");

            if (MainBranch != null)
                sb.AppendLine(Display.Utils.Indent(level + 1) + $"Main Branch: {MainBranch.Pretty(level + 1)}");
            if (ElseBranch != null)
                sb.AppendLine(Display.Utils.Indent(level + 1) + $"Else Branch: {ElseBranch.Pretty(level + 1)}");

            return sb.ToString();
        }
    }
}