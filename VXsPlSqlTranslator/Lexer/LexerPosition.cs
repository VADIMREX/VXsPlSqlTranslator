namespace VXs.Lexer;

public class LexerPosition
{
    public int i = 0;
    public int pos = 0;
    public int line = 0;
    public int col = 0;

    public int next => i + 1;

    public void NewLine()
    {
        line++;
        col = 0;
    }

    public static LexerPosition operator ++(LexerPosition self)
    {
        self.i++;
        self.pos++;
        self.col++;
        return self;
    }

    public static LexerPosition operator +(LexerPosition self, int arg)
    {
        self.i += arg;
        self.pos += arg;
        self.col += arg;
        return self;
    }

    public static LexerPosition operator --(LexerPosition self)
    {
        self.i--;
        self.pos--;
        self.col--;
        return self;
    }

    public static LexerPosition operator -(LexerPosition self, int arg)
    {
        self.i -= arg;
        self.pos -= arg;
        self.col -= arg;
        return self;
    }

    public static implicit operator int(LexerPosition self) => self.i;
}