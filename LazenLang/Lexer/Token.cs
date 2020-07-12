using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LazenLang.Lexer
{
    struct CodePosition
    {
        public int Line { get; set; }
        public int Column { get; set; }

        public CodePosition(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }

    class Token
    {
        public string Value { get; set;  }
        public TokenType Type { get; set; }
        public CodePosition Pos { get; set; }

        public Token(string value, TokenType type, CodePosition pos)
        {
            Value = value;
            Type = type;
            Pos = pos;
        }
    }
}
