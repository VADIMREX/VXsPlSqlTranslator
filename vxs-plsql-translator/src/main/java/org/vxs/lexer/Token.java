package org.vxs.lexer;

public class Token {
    public TokenType Type;
    public String Kind;
    public int Line = 0;
    public int Col = 0;
    public int Pos = 0;
    public String Text = "";

    public Token(TokenType type,
                 int line,
                 int col,
                 int pos,
                 String text)
    {
        Type = type;
        Line = line;
        Col = col;
        Pos = pos;
        Text = text;
    }

    @Override
    public String toString() {
        return String.format("%d,%d [%s]: %s", Line, Col, Type, Text);
    }

    public Token clone() {
        return new Token(Type, Line, Col, Pos, Text);
    }
}
