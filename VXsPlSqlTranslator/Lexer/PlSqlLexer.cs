using System.Text;

namespace VXs.Lexer;

public class PlSqlLexer
{
    class Position
    {
        public int i = 0;
        public int pos = 0;
        public int line = 0;
        public int col = 0;

        public int next => i + 1;

        public static Position operator ++(Position self)
        {
            self.i++;
            self.pos++;
            self.col++;
            return self;
        }

        public static Position operator +(Position self, int arg)
        {
            self.i += arg;
            self.pos += arg;
            self.col += arg;
            return self;
        }

        public static Position operator -(Position self, int arg)
        {
            self.i -= arg;
            self.pos -= arg;
            self.col -= arg;
            return self;
        }

        public static Position operator --(Position self)
        {
            self.i--;
            self.pos--;
            self.col--;
            return self;
        }

        public static Position operator -(Position self)
        {
            self.i++;
            self.pos++;
            self.line++;
            self.col = 0;
            return self;
        }

        public static implicit operator int(Position self) => self.i;
    }

    Token? InlineComment(Position pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "-");

        if ('-' != source[pos.next])
        {
            pos--;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "-");
        }

        var buffer = new StringBuilder();
        for (pos += 2; pos < source.Length; pos++)
        {
            var c = source[pos];
            if ('\n' == c)
            {
                break;
            }
            buffer.Append(c);
        }
        return new Token(TokenType.Commentary, "inline", pos.line, pos.col, pos.i, buffer.ToString());
    }

    Token? MultilineComment(Position pos, string source) {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "/");

        if ('*' != source[pos.next])
        {
            pos--;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "/");
        }

        var buffer = new StringBuilder();
        for (pos += 2; pos < source.Length; pos++)
        {
            var c = source[pos];
            if ('*' == c)
            {
                if (pos.next >= source.Length) break;
                if ('/' == source[pos.next])
                {
                    pos++;
                    return new Token(TokenType.Commentary, "multiline", pos.line, pos.col, pos.i, buffer.ToString());
                }
            }
            buffer.Append(c);
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.i, "unclosed commentary");
    }

    public IEnumerable<Token> Parse(string source)
    {
        var pos = new Position();
        Token? token = null;
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            switch (c)
            {
                case '+':
                case '*':
                    token = new Token(TokenType.Operator, pos.line, pos.col, pos.pos, $"{c}");
                    break;
                case '-':
                    token = InlineComment(pos, source);
                    break;
                case '/':
                    token = MultilineComment(pos, source);
                    break;
            }

            if (null != token)
            {
                yield return token;
                token = null;
            }
        }
    }
}