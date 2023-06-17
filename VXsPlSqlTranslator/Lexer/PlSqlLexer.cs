using System.Text;

namespace VXs.Lexer;

public class PlSqlLexer
{
    Token? InlineComment(LexerPosition pos, string source)
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

    Token? MultilineComment(LexerPosition pos, string source)
    {
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
            if ('\n' == c)
            {
                pos.NewLine();
            }
            else if ('*' == c)
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

    Token? LessOrEqual(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "<");
        if ('=' == source[pos.next]){
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "<=");
        }
        if ('>' == source[pos.next]){
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "<>");
        }
        if ('<' == source[pos.next])
        {
            pos+=2;
            var token = Identifier(pos, source);
            if (null == token) return new Token(TokenType.Error, pos.line, pos.col, pos.i, "identifier expected");
            if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, ">> expected");
            if (pos.next + 1 >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, ">> expected");
            if (">>" != source.Substring(pos, 2)) return new Token(TokenType.Error, pos.line, pos.col, pos.i, ">> expected");
            token.Text = $"<<{token.Text}>>";
            return token;
        }
        pos--;
        return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "<");
    }

    Token? Exponential(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "*");
        if ('*' == source[pos.next]){
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "**");
        }
        pos--;
        return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "*");
    }

    Token? GreaterOrEqual(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.i, ">");
        if ('=' == source[pos.next]){
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, ">=");
        }
        pos--;
        return new Token(TokenType.Operator, pos.line, pos.col, pos.i, ">");
    }

    Token? NotEqual(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, "!");
        if ('=' == source[pos.next]){
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "!=");
        }
        pos--;
        return new Token(TokenType.Error, pos.line, pos.col, pos.i, "!");
    }

    Token? Assign(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, ":");
        if ('=' == source[pos.next]){
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, ":=");
        }
        pos--;
        return new Token(TokenType.Error, pos.line, pos.col, pos.i, ":");
    }

    Token? Concat(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, "|");
        if ('|' == source[pos.next]) {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "||");
        }
        pos--;
        return new Token(TokenType.Error, pos.line, pos.col, pos.i, "|");
    }

    Token? Identifier(LexerPosition pos, string source)
    {
        var sb = new StringBuilder();
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if (char.IsWhiteSpace(c)) return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            if (!char.IsAsciiLetterOrDigit(c) && '_' != c && '#' != c && '$' != c) {
                pos--;
                return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            }
            sb.Append(c);
        }
        return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
    }

    Token? Range(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, ".");
        if ('.' == source[pos.next])
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "..");
        pos--;
        return new Token(TokenType.Error, pos.line, pos.col, pos.i, ".");
    }

    Token? Relational1(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, "~");
        if ('~' == source[pos.next])
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "~=");
        pos--;
        return new Token(TokenType.Error, pos.line, pos.col, pos.i, "~");
    }

    Token? Relational2(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.i, "^");
        if ('=' == source[pos.next])
            return new Token(TokenType.Operator, pos.line, pos.col, pos.i, "^=");
        pos--;
        return new Token(TokenType.Error, pos.line, pos.col, pos.i, "^");
    }

    #warning TODO
    Token? Literal(LexerPosition pos, string source)
    {
        var sb = new StringBuilder();
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if (char.IsWhiteSpace(c)) return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            if (!char.IsAsciiLetterOrDigit(c) && '_' != c && '#' != c && '$' != c) {
                pos--;
                return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            }
            sb.Append(c);
        }
        return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
    }

    #warning TODO
    Token? CaseSensetiveIdentifier(LexerPosition pos, string source)
    {
        var sb = new StringBuilder();
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if (char.IsWhiteSpace(c)) return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            if (!char.IsAsciiLetterOrDigit(c) && '_' != c && '#' != c && '$' != c) {
                pos--;
                return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            }
            sb.Append(c);
        }
        return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
    }

    #warning TODO
    Token? Number(LexerPosition pos, string source)
    {
        var sb = new StringBuilder();
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if (char.IsWhiteSpace(c)) return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            if (!char.IsAsciiLetterOrDigit(c) && '_' != c && '#' != c && '$' != c) {
                pos--;
                return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
            }
            sb.Append(c);
        }
        return new Token(TokenType.Name, "case insensetive", pos.line, pos.col, pos.i, sb.ToString());
    }

    public IEnumerable<Token> Parse(string source)
    {
        var pos = new LexerPosition();
        Token? token = null;
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            switch (c)
            {
                case '\n':
                    pos.NewLine();
                    break;
                // operators
                case '+':
                case '=':
                    token = new Token(TokenType.Operator, pos.line, pos.col, pos.pos, $"{c}");
                    break;
                case '*':
                    token = Exponential(pos, source);
                    break;
                case '-':
                    token = InlineComment(pos, source);
                    break;
                case '/':
                    token = MultilineComment(pos, source);
                    break;
                case '<':
                    token = LessOrEqual(pos, source);
                    break;
                case '>':
                    token = GreaterOrEqual(pos, source);
                    break;
                case '!':
                    token = NotEqual(pos, source);
                    break;
                case ':':
                    token = Assign(pos, source);
                    break;
                case '|':
                    token = Concat(pos, source);
                    break;
                case '.':
                    token = Range(pos, source);
                    break;
                case '~':
                    token = Relational1(pos, source);
                    break;
                case '^':
                    token = Relational2(pos, source);
                    break;
                // expression or list delimiter
                case '(':
                case ')':
                // statement terminator
                case ';':
                // item separator
                case ',':
                // remote access
                case '@':
                // attribute indicator
                case '%':
                    token = new Token(TokenType.Special, pos.line, pos.col, pos.pos, $"{c}");
                    break;
                // invalid
                case '&':
                case '{':
                case '}':
                case '?':
                case '[':
                case ']':
                    token = new Token(TokenType.Error, pos.line, pos.col, pos.pos, $"{c}");
                    break;
                // literal
                case '\'':
                    token = Literal(pos, source);
                    break;
                // case sensetive
                case '"':
                    token = CaseSensetiveIdentifier(pos, source);
                    break;
                // identifier part
                case '#':
                case '$':
                case '_':
                    break;
                //  # $ & _ | { } ? [ ]
                default:
                    if (char.IsAsciiLetter(c)) token = Identifier(pos, source);
                    else if (char.IsDigit(c)) token = Number(pos, source);
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