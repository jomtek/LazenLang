using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LazenLang.Lexing;
using LazenLang.Parsing;
using LazenLang.Parsing.Ast;
using LazenLang.Parsing.Ast.Statements;

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

            Block topLevel;
            try
            {
                topLevel = Block.Consume(parser, false, true);
            } catch (ParserError ex)
            {
                string prettyPrintedPos = $"{ex.Position.Line}:{ex.Position.Column}";
                string prettyPrintedContent = PrettyPrinter.ParserExceptions.PrettyContent(ex.Content);
                Console.WriteLine($"error: {prettyPrintedPos}: {ex.Content.GetType().Name}: {prettyPrintedContent}");
                return;
            }

            stopwatch.Stop();
            
            foreach (InstrNode instr in topLevel.Instructions)
            {
                Console.WriteLine(instr.Value.Pretty());
            }

            Console.WriteLine("Elapsed " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}