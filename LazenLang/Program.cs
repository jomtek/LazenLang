using System;
using System.Collections.Generic;
using System.IO;
using LazenLang.Lexing;
using LazenLang.Parsing;

namespace LazenLang
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = File.ReadAllText("code.lzn");
            List<Token> tokens = new Lexer(code).tokens;
            Parser parser = new Parser(tokens);

            foreach (Token token in tokens)
                Console.WriteLine(token.ToString());
        }
    }
}
