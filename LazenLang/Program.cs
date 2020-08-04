using System;
using System.Collections.Generic;
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
            string code = File.ReadAllText("code.lzn");
            List<Token> tokens = new Lexer(code).tokens;
            Parser parser = new Parser(tokens);

            Block topLevel = Block.Consume(parser, false);
            
        }
    }
}