using System;
using LazenLang.Lexing;
using System.Collections.Generic;
using System.Text;
using LazenLang.Parsing.Ast.Statements;
using System.Linq;

namespace LazenLang.Parsing.Ast.Expressions
{
    class IfInstr : Expr
    {
        public Expr Condition;
        public Block MainBranch;
        public Block ElseBranch;

        public IfInstr(Expr condition, Block mainBranch, Block elseBranch)
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
            Expr baseCondition;
            try
            {
                baseCondition = parser.TryConsumer(ExprNode.Consume).Value;
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid expression after IF token"),
                    parser.Cursor
                );
            }
            #endregion

            #region main_branch
            Instr mainInstr;
            try
            {
                mainInstr = parser.TryConsumer(InstrNode.Consume);
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid block in IF instruction"),
                    parser.Cursor
                );
            }

            if (mainInstr is Block)
                mainBranch = (Block)mainInstr;
            else
                mainBranch = new Block(new Instr[] { mainInstr });
            #endregion

            #region elif_branches_collect
            while (true)
            {
                Expr condition;
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
                } catch (ParserError)
                {
                    break;
                }

                try
                {
                    condition = parser.TryConsumer(ExprNode.Consume).Value;
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
            } catch (ParserError)
            {
                isThereElse = false;
            }

            if (isThereElse)
            {
                Instr elseInstr;

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

                if (elseInstr is Block)
                    elseBranch = (Block)elseInstr;
                else
                    elseBranch = new Block(new Instr[] { elseInstr });

                if (elifBranches.Count > 0)
                    elifBranches[elifBranches.Count - 1].ElseBranch = elseBranch;
            }
            #endregion

            #region elif_else_fold
            if (elifBranches.Count > 0)
            {
                elifBranches.Reverse();
                for (int i = 0; i < elifBranches.Count - 1; i++)
                {
                    IfInstr branch = elifBranches[i];
                    elifBranches[i + 1].ElseBranch = new Block(new Instr[] { new ExprInstr(branch) });
                }
            }

            if (elifBranches.Count > 0)
            {
                elseBranch = new Block(new Instr[] { new ExprInstr(elifBranches[elifBranches.Count - 1]) });
            }
            #endregion

            return new IfInstr(baseCondition, mainBranch, elseBranch);
        }

        public override string Pretty()
        {
            string mainBranchPretty = "none";
            string elseBranchPretty = "none";

            if (MainBranch != null)
                mainBranchPretty = MainBranch.Pretty();

            if (ElseBranch != null)
                elseBranchPretty = ElseBranch.Pretty();

            return $"IfInstr(condition: {Condition.Pretty()}, main: {mainBranchPretty}, else: {elseBranchPretty})";
        }
    }
}