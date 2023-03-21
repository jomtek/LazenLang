using LazenLang.Parsing.Ast.Statements;
using LazenLang.Parsing.Ast.Types;
using Parsing.Ast;
using System;
using System.Collections.Generic;
using System.Text;
using Typechecking.Errors;

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
                        string name = varDecl.Name.Value;

                        if (varDecl.Type == null)
                        {
                            // Update value
                            // Example: baz = True
                            _context.AssertExists(name, node.Position);
                            TypeDesc contextType = _context.GetTypeUnsafe(name);

                            // Make type matching
                            TypeDesc guessedType = TypeInvestigator.Investigate(varDecl.Value, _context);
                            if (!TypeComparator.Compare(contextType, guessedType))
                            {
                                /*
                                baz: Int = 5;
                                baz = True;
                                You assign baz to a Bool value but it is registered as an Int
                                */
                                
                                var expected = TypeDisplayer.Pretty(contextType);
                                var got = TypeDisplayer.Pretty(guessedType);
                                throw new TypeCheckerError(new TypesMismatched(expected, got), node.Position);
                            }
                        }
                        else
                        {
                            // Make type matching between value type and explicit type
                            TypeDesc guessedType = TypeInvestigator.Investigate(varDecl.Value, _context);
                            if (!TypeComparator.Compare(varDecl.Type.Value, guessedType))
                            {
                                /*
                                baz: Int = "hello, world!";
                                You assign baz to a String value but its type is Int
                                */
                                var expected = TypeDisplayer.Pretty(varDecl.Type.Value);
                                var got = TypeDisplayer.Pretty(guessedType);
                                throw new TypeCheckerError(new TypesMismatched(expected, got), node.Position);
                            }

                            _context.Add(name, varDecl.Type.Value, node.Position);
                        }

                        Console.WriteLine("Vardecl");
                        break;
                    default:
                        Console.WriteLine("Unkown");
                        break;
                }
            }
        }
    }
}
