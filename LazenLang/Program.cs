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
            List<Token> tokens = new Lexer(code).tokens;
            Parser parser = new Parser(tokens);

            Block topLevel = Block.Consume(parser, false);

            stopwatch.Stop();
            
            foreach (Instr instr in topLevel.Instructions)
            {
                Console.WriteLine(instr.Pretty());
            }

            Console.WriteLine("Elapsed " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }
}