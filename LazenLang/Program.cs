using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LazenLang.Lexing;
//using LazenLang.Parsing;
//using LazenLang.Parsing.Ast.Statements;
//using Parsing.Errors;

namespace LazenLang
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Compiling..."); 

            string code = File.ReadAllText("program.lzn");

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Token[] tokens = new Lexer(code).Tokens;
            foreach (Token tok in tokens)
            {
                Console.WriteLine(tok.ToString());
            }
            
            /*Parser parser = new Parser(tokens);

            Block ast = null;
            try
            {
                ast = Block.Consume(parser, false, true);
            }
            catch (ParserError ex)
            {
                if (ex.Content is NoTokenLeft == false)
                {
                    string prettyPrintedPos = $"{ex.Position.Line}:{ex.Position.Column}";
                    string prettyPrintedContent = Parsing.Display.PrettyExceptions.PrettyContent(ex.Content);
                    Console.WriteLine(
                        $"error: {prettyPrintedPos}: {ex.Content.GetType().Name}: {prettyPrintedContent}");
                    return;
                }
            }

            if (ast == null)
            {
                return;
            }
            
            Console.WriteLine("PrettyPrinting:\n\n");
            Console.WriteLine(Parsing.Display.Utils.PostProcessResult(ast.Pretty(0)));*/

            //TopLevelChecker typechecker = new TopLevelChecker(ast);
            //typechecker.Typecheck();

            //stopwatch.Stop();

            /*foreach (InstrNode instr in ast.Instructions)
            {
                Console.WriteLine(instr.Value.Pretty());
            }*/

            //Console.WriteLine(IPrettyPrintable.PrettyBlock(ast, 0));

	    // IR compiler.

            Console.WriteLine("Elapsed " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}