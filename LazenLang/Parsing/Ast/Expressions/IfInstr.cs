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
            Block mainBranch = null;
            Block elseBranch = null;
            var elifBranches = new List<IfInstr>();

            parser.Eat(TokenInfo.TokenType.IF);
            
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

            try
            {
                mainBranch = parser.TryConsumer((Parser p) => Block.Consume(p));
            } catch (ParserError ex)
            {
                if (!ex.IsExceptionFictive()) throw ex;
                throw new ParserError(
                    new InvalidElementException("Invalid block in IF instruction"),
                    parser.Cursor
                );
            }

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
                try
                {
                    elseBranch = parser.TryConsumer((Parser p) => Block.Consume(p));
                }
                catch (ParserError ex)
                {
                    if (!ex.IsExceptionFictive()) throw ex;
                    throw new ParserError(
                        new InvalidElementException("Invalid block in ELSE instruction"),
                        parser.Cursor
                    );
                }

                if (elifBranches.Count > 0)
                    elifBranches[elifBranches.Count - 1].ElseBranch = elseBranch;
            }


            if (elifBranches.Count > 0)
            {
                for (int i = 0; i < elifBranches.Count - 1; i++)
                {
                    IfInstr branch = elifBranches[i];
                    elifBranches[i + 1].ElseBranch = new Block(new Instr[] { new ExprInstr(branch) });
                }
            }

            if (elifBranches.Count > 0)
            {
                Console.WriteLine("else branch OK");
                elseBranch = new Block(new Instr[] { new ExprInstr(elifBranches[0]) });
            }
            /*Console.WriteLine("basecondition : " + baseCondition.Pretty());
            Console.WriteLine("mainbranch (" + mainBranch.Instructions.Count().ToString() + "): ");
            foreach (Instr x in mainBranch.Instructions)
                Console.WriteLine(x.Pretty());

            Console.WriteLine("elsebranch (" + elseBranch.Instructions.Count().ToString() + "): ");
            foreach (Instr x in elseBranch.Instructions)
                Console.WriteLine(x.Pretty());*/

            return new IfInstr(baseCondition, mainBranch, elseBranch);
        }

        public override string Pretty()
        {
            string mainBranchPretty = "none";
            string elseBranchPretty = "none";

            if (MainBranch != null)
                mainBranchPretty = InstrNode.PrettyMultiple(MainBranch.Instructions);

            if (ElseBranch != null)
                elseBranchPretty = InstrNode.PrettyMultiple(ElseBranch.Instructions);

            return $"IfInstr(condition: {Condition.Pretty()}, main: {mainBranchPretty}, else: {elseBranchPretty})";
        }
    }
}