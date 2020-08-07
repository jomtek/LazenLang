namespace LazenLang.Lexing
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

        public string Pretty()
        {
            return $"line: {Line}, column: {Column}";
        }
    }

    class Token
    {
        public string Value { get; set;  }
        public TokenInfo.TokenType Type { get; set; }
        public CodePosition Pos { get; set; }

        public Token(string value, TokenInfo.TokenType type, CodePosition pos)
        {
            Value = value;
            Type = type;
            Pos = pos;
        }

        public override string ToString()
        {
            if (Type != TokenInfo.TokenType.EOL)
            {
                return $"{Type} - {Pos.Line}: {Pos.Column} - value: {Value}";
            } else
            {
                return "EOL";
            }
        }
    }
}
