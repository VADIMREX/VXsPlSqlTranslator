namespace VXs.Lexer;

public class Token
{
    public TokenType Type;
    public String Kind;
    public int Line = 0;
    public int Col = 0;
    public int Pos = 0;
    public String Text = "";

    public Token(TokenType type,
                 string kind,
                 int line,
                 int col,
                 int pos,
                 string text)
    {
        Type = type;
        Kind = kind;
        Line = line;
        Col = col;
        Pos = pos;
        Text = text;
    }

    public Token(TokenType type,
                 int line,
                 int col,
                 int pos,
                 string text) : this(type, "", line, col, pos, text) { }

    public override string ToString() => string.Format("{0},{1} [{2}]: {3}", Line, Col, Type, Text);

    public Token Clone() => new Token(Type, Kind, Line, Col, Pos, Text);

    public static readonly Token None = new Token(TokenType.None, 0, 0, 0, "");
}
