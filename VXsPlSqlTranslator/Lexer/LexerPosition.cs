namespace VXs.Lexer;

public class LexerPosition
{
    public int pos = 0;
    public int line = 0;
    public int col = 0;

    public int next => pos + 1;

    public void NewLine()
    {
        line++;
        col = -1;
    }

    public LexerPosition Clone() => new LexerPosition { pos = pos, line = line, col = col };

    public static LexerPosition operator ++(LexerPosition self)
    {
        self.pos++;
        self.col++;
        return self;
    }

    public static LexerPosition operator +(LexerPosition self, int arg)
    {
        self.pos += arg;
        self.col += arg;
        return self;
    }

    public static LexerPosition operator --(LexerPosition self)
    {
        self.pos--;
        self.col--;
        return self;
    }

    public static LexerPosition operator -(LexerPosition self, int arg)
    {
        self.pos -= arg;
        self.col -= arg;
        return self;
    }

    public static implicit operator int(LexerPosition self) => self.pos;

    public override string ToString() => $"Ln {line}, Col {col} ({pos} pos)";
}