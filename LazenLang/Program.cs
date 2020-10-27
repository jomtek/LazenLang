using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LazenLang.Lexing;
using LazenLang.Parsing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Statements;
using LazenLang.Typechecking;

namespace LazenLang
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Compiling...");
            
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            string code = File.ReadAllText("../../code.lzn");
            Token[] tokens = new Lexer(code).Tokens;
            Parser parser = new Parser(tokens);

            Block ast;
            try
            {
                ast = Block.Consume(parser, false, true);
            } catch (ParserError ex)
            {
                if (!(ex.Content is NoTokenLeft))
                {
                    string prettyPrintedPos = $"{ex.Position.Line}:{ex.Position.Column}";
                    string prettyPrintedContent = PrettyPrinter.PrettyExceptions.PrettyContent(ex.Content);
                    Console.WriteLine($"error: {prettyPrintedPos}: {ex.Content.GetType().Name}: {prettyPrintedContent}");
                }
                return;
            }

            TopLevelChecker typechecker = new TopLevelChecker(ast);
            typechecker.Typecheck();

            stopwatch.Stop();
            
            foreach (InstrNode instr in ast.Instructions)
            {
                Console.WriteLine(instr.Value.Pretty());
            }

	    // IR compiler.

            Console.WriteLine("Elapsed " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}