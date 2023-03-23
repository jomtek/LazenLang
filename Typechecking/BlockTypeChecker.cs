using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Expressions;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Statements.Functions;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using Parsing.Errors;
using System;
using System.Collections.Generic;
using Typechecking.Checkers;
using Typechecking.Errors;
using Typechecking.Investigation;

namespace Typechecking
{
    public class BlockTypeChecker
    {
        private Block _block;
        private LocalContext _context;

        public BlockTypeChecker(Block block, LocalContext context)
        {
            _block = block;
            _context = context;
        }

        public void Check()
        {
            foreach (var node in _block.Instructions)
            {
                switch (node.Value)
                {
                    case VarDecl varDecl:
                        VarDeclChecker.Check(varDecl, _context, node.Position);
                        break;

                    case FuncDecl funcDecl:
                        FuncDeclChecker.Check(funcDecl, _context, node.Position);
                        break;

                    case ReturnInstr returnInstr:
                        ReturnInstrChecker.Check(returnInstr, _context, node.Position);
                        break;

                    case ExprInstr exprInstr:
                        var enode = new ExprNode(exprInstr.Expression, node.Position);
                        TypeInvestigator.Investigate(enode, _context);
                        break;

                    default:
                        throw new NotSupportedException();
                }
            }
        }
    }
}
