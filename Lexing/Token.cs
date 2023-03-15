namespace LazenLang.Lexing
{
    public struct CodePosition
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

    public class Token
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
            return $"{Type} - {Pos.Line}: {Pos.Column} - value: {Value}";
        }
    }
}
