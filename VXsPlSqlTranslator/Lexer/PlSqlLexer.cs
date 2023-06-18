using System.Text;

namespace VXs.Lexer;

public class PlSqlLexer
{
    Dictionary<string, (TokenType type, string kind)> Keywords = new()
    {
        { "ALL", (TokenType.Keyword, "") },
        { "ALTER", (TokenType.Keyword, "") },
        { "AND", (TokenType.Operator, "") },
        { "ANY", (TokenType.Keyword, "") },
        { "AS", (TokenType.Keyword, "") },
        { "ASC", (TokenType.Keyword, "") },
        { "AT", (TokenType.Keyword, "") },
        { "BEGIN", (TokenType.Keyword, "") },
        { "BETWEEN", (TokenType.Keyword, "") },
        { "BY", (TokenType.Keyword, "") },
        { "CASE", (TokenType.Keyword, "") },
        { "CHECK", (TokenType.Keyword, "") },
        { "CLUSTERS", (TokenType.Keyword, "") },
        { "CLUSTER", (TokenType.Keyword, "") },
        { "COLAUTH", (TokenType.Keyword, "") },
        { "COLUMNS", (TokenType.Keyword, "") },
        { "COMPRESS", (TokenType.Keyword, "") },
        { "CONNECT", (TokenType.Keyword, "") },
        { "CRASH", (TokenType.Keyword, "") },
        { "CREATE", (TokenType.Keyword, "") },
        { "CURSOR", (TokenType.Keyword, "") },
        { "DECLARE", (TokenType.Keyword, "") },
        { "DEFAULT", (TokenType.Keyword, "") },
        { "DESC", (TokenType.Keyword, "") },
        { "DISTINCT", (TokenType.Keyword, "") },
        { "DROP", (TokenType.Keyword, "") },
        { "ELSE", (TokenType.Keyword, "") },
        { "END", (TokenType.Keyword, "") },
        { "EXCEPTION", (TokenType.Keyword, "") },
        { "EXCLUSIVE", (TokenType.Keyword, "") },
        { "FETCH", (TokenType.Keyword, "") },
        { "FOR", (TokenType.Keyword, "") },
        { "FROM", (TokenType.Keyword, "") },
        { "FUNCTION", (TokenType.Keyword, "") },
        { "GOTO", (TokenType.Keyword, "") },
        { "GRANT", (TokenType.Keyword, "") },
        { "GROUP", (TokenType.Keyword, "") },
        { "HAVING", (TokenType.Keyword, "") },
        { "IDENTIFIED", (TokenType.Keyword, "") },
        { "IF", (TokenType.Keyword, "") },
        { "IN", (TokenType.Keyword, "") },
        { "INDEX", (TokenType.Keyword, "") },
        { "INDEXES", (TokenType.Keyword, "") },
        { "INSERT", (TokenType.Keyword, "") },
        { "INTERSECT", (TokenType.Keyword, "") },
        { "INTO", (TokenType.Keyword, "") },
        { "IS", (TokenType.Keyword, "") },
        { "LIKE", (TokenType.Keyword, "") },
        { "LOCK", (TokenType.Keyword, "") },
        { "MINUS", (TokenType.Keyword, "") },
        { "MODE", (TokenType.Keyword, "") },
        { "NOCOMPRESS", (TokenType.Keyword, "") },
        { "NOT", (TokenType.Operator, "") },
        { "NOWAIT", (TokenType.Keyword, "") },
        { "NULL", (TokenType.Value, "null") },
        { "OF", (TokenType.Keyword, "") },
        { "ON", (TokenType.Keyword, "") },
        { "OPTION", (TokenType.Keyword, "") },
        { "OR", (TokenType.Operator, "") },
        { "ORDER", (TokenType.Keyword, "") },
        { "OVERLAPS", (TokenType.Keyword, "") },
        { "PROCEDURE", (TokenType.Keyword, "") },
        { "PUBLIC", (TokenType.Keyword, "") },
        { "RESOURCE", (TokenType.Keyword, "") },
        { "REVOKE", (TokenType.Keyword, "") },
        { "SELECT", (TokenType.Keyword, "") },
        { "SHARE", (TokenType.Keyword, "") },
        { "SIZE", (TokenType.Keyword, "") },
        { "SQL", (TokenType.Keyword, "") },
        { "START", (TokenType.Keyword, "") },
        { "SUBTYPE", (TokenType.Keyword, "") },
        { "TABAUTH", (TokenType.Keyword, "") },
        { "TABLE", (TokenType.Keyword, "") },
        { "THEN", (TokenType.Keyword, "") },
        { "TO", (TokenType.Keyword, "") },
        { "TYPE", (TokenType.Keyword, "") },
        { "UNION", (TokenType.Keyword, "") },
        { "UNIQUE", (TokenType.Keyword, "") },
        { "UPDATE", (TokenType.Keyword, "") },
        { "VALUES", (TokenType.Keyword, "") },
        { "VIEW", (TokenType.Keyword, "") },
        { "VIEWS", (TokenType.Keyword, "") },
        { "WHEN", (TokenType.Keyword, "") },
        { "WHERE", (TokenType.Keyword, "") },
        { "WITH", (TokenType.Keyword, "") },
        { "A", (TokenType.Keyword, "") },
        { "ADD", (TokenType.Keyword, "") },
        { "ACCESSIBLE", (TokenType.Keyword, "") },
        { "AGENT", (TokenType.Keyword, "") },
        { "AGGREGATE", (TokenType.Keyword, "") },
        { "ARRAY", (TokenType.Keyword, "") },
        { "ATTRIBUTE", (TokenType.Keyword, "") },
        { "AUTHID", (TokenType.Keyword, "") },
        { "AVG", (TokenType.Keyword, "") },
        { "BFILE_BASE", (TokenType.Keyword, "") },
        { "BINARY", (TokenType.Keyword, "") },
        { "BLOB_BASE", (TokenType.Keyword, "") },
        { "BLOCK", (TokenType.Keyword, "") },
        { "BODY", (TokenType.Keyword, "") },
        { "BOTH", (TokenType.Keyword, "") },
        { "BOUND", (TokenType.Keyword, "") },
        { "BULK", (TokenType.Keyword, "") },
        { "BYTE", (TokenType.Keyword, "") },
        { "C", (TokenType.Keyword, "") },
        { "CALL", (TokenType.Keyword, "") },
        { "CALLING", (TokenType.Keyword, "") },
        { "CASCADE", (TokenType.Keyword, "") },
        { "CHAR", (TokenType.Keyword, "") },
        { "CHAR_BASE", (TokenType.Keyword, "") },
        { "CHARACTER", (TokenType.Keyword, "") },
        { "CHARSET", (TokenType.Keyword, "") },
        { "CHARSETFORM", (TokenType.Keyword, "") },
        { "CHARSETID", (TokenType.Keyword, "") },
        { "CLOB_BASE", (TokenType.Keyword, "") },
        { "CLONE", (TokenType.Keyword, "") },
        { "CLOSE", (TokenType.Keyword, "") },
        { "COLLECT", (TokenType.Keyword, "") },
        { "COMMENT", (TokenType.Keyword, "") },
        { "COMMIT", (TokenType.Keyword, "") },
        { "COMMITTED", (TokenType.Keyword, "") },
        { "COMPILED", (TokenType.Keyword, "") },
        { "CONSTANT", (TokenType.Keyword, "") },
        { "CONSTRUCTOR", (TokenType.Keyword, "") },
        { "CONTEXT", (TokenType.Keyword, "") },
        { "CONTINUE", (TokenType.Keyword, "") },
        { "CONVERT", (TokenType.Keyword, "") },
        { "COUNT", (TokenType.Keyword, "") },
        { "CREDENTIAL", (TokenType.Keyword, "") },
        { "CURRENT", (TokenType.Keyword, "") },
        { "CUSTOMDATUM", (TokenType.Keyword, "") },
        { "DANGLING", (TokenType.Keyword, "") },
        { "DATA", (TokenType.Keyword, "") },
        { "DATE", (TokenType.Keyword, "") },
        { "DATE_BASE", (TokenType.Keyword, "") },
        { "DAY", (TokenType.Keyword, "") },
        { "DEFINE", (TokenType.Keyword, "") },
        { "DELETE", (TokenType.Keyword, "") },
        { "DETERMINISTIC", (TokenType.Keyword, "") },
        { "DIRECTORY", (TokenType.Keyword, "") },
        { "DOUBLE", (TokenType.Keyword, "") },
        { "DURATION", (TokenType.Keyword, "") },
        { "ELEMENT", (TokenType.Keyword, "") },
        { "ELSIF", (TokenType.Keyword, "") },
        { "EMPTY", (TokenType.Keyword, "") },
        { "ESCAPE", (TokenType.Keyword, "") },
        { "EXCEPT", (TokenType.Keyword, "") },
        { "EXCEPTIONS", (TokenType.Keyword, "") },
        { "EXECUTE", (TokenType.Keyword, "") },
        { "EXISTS", (TokenType.Keyword, "") },
        { "EXIT", (TokenType.Keyword, "") },
        { "EXTERNAL", (TokenType.Keyword, "") },
        { "FINAL", (TokenType.Keyword, "") },
        { "FIRST", (TokenType.Keyword, "") },
        { "FIXED", (TokenType.Keyword, "") },
        { "FLOAT", (TokenType.Keyword, "") },
        { "FORALL", (TokenType.Keyword, "") },
        { "FORCE", (TokenType.Keyword, "") },
        { "GENERAL", (TokenType.Keyword, "") },
        { "HASH", (TokenType.Keyword, "") },
        { "HEAP", (TokenType.Keyword, "") },
        { "HIDDEN", (TokenType.Keyword, "") },
        { "HOUR", (TokenType.Keyword, "") },
        { "IMMEDIATE", (TokenType.Keyword, "") },
        { "INCLUDING", (TokenType.Keyword, "") },
        { "INDICATOR", (TokenType.Keyword, "") },
        { "INDICES", (TokenType.Keyword, "") },
        { "INFINITE", (TokenType.Keyword, "") },
        { "INSTANTIABLE", (TokenType.Keyword, "") },
        { "INT", (TokenType.Keyword, "") },
        { "INTERFACE", (TokenType.Keyword, "") },
        { "INTERVAL", (TokenType.Keyword, "") },
        { "INVALIDATE", (TokenType.Keyword, "") },
        { "ISOLATION", (TokenType.Keyword, "") },
        { "JAVA", (TokenType.Keyword, "") },
        { "LANGUAGE", (TokenType.Keyword, "") },
        { "LARGE", (TokenType.Keyword, "") },
        { "LEADING", (TokenType.Keyword, "") },
        { "LENGTH", (TokenType.Keyword, "") },
        { "LEVEL", (TokenType.Keyword, "") },
        { "LIBRARY", (TokenType.Keyword, "") },
        { "LIKE2", (TokenType.Keyword, "") },
        { "LIKE4", (TokenType.Keyword, "") },
        { "LIKEC", (TokenType.Keyword, "") },
        { "LIMIT", (TokenType.Keyword, "") },
        { "LIMITED", (TokenType.Keyword, "") },
        { "LOCAL", (TokenType.Keyword, "") },
        { "LONG", (TokenType.Keyword, "") },
        { "LOOP", (TokenType.Keyword, "") },
        { "MAP", (TokenType.Keyword, "") },
        { "MAX", (TokenType.Keyword, "") },
        { "MAXLEN", (TokenType.Keyword, "") },
        { "MEMBER", (TokenType.Keyword, "") },
        { "MERGE", (TokenType.Keyword, "") },
        { "MIN", (TokenType.Keyword, "") },
        { "MINUTE", (TokenType.Keyword, "") },
        { "MOD", (TokenType.Keyword, "") },
        { "MODIFY", (TokenType.Keyword, "") },
        { "MONTH", (TokenType.Keyword, "") },
        { "MULTISET", (TokenType.Keyword, "") },
        { "NAME", (TokenType.Keyword, "") },
        { "NAN", (TokenType.Keyword, "") },
        { "NATIONAL", (TokenType.Keyword, "") },
        { "NATIVE", (TokenType.Keyword, "") },
        { "NCHAR", (TokenType.Keyword, "") },
        { "NEW", (TokenType.Keyword, "") },
        { "NOCOPY", (TokenType.Keyword, "") },
        { "NUMBER_BASE", (TokenType.Keyword, "") },
        { "OBJECT", (TokenType.Keyword, "") },
        { "OCICOLL", (TokenType.Keyword, "") },
        { "OCIDATE", (TokenType.Keyword, "") },
        { "OCIDATETIME", (TokenType.Keyword, "") },
        { "OCIDURATION", (TokenType.Keyword, "") },
        { "OCIINTERVAL", (TokenType.Keyword, "") },
        { "OCILOBLOCATOR", (TokenType.Keyword, "") },
        { "OCINUMBER", (TokenType.Keyword, "") },
        { "OCIRAW", (TokenType.Keyword, "") },
        { "OCIREF", (TokenType.Keyword, "") },
        { "OCIREFCURSOR", (TokenType.Keyword, "") },
        { "OCIROWID", (TokenType.Keyword, "") },
        { "OCISTRING", (TokenType.Keyword, "") },
        { "OCITYPE", (TokenType.Keyword, "") },
        { "OLD", (TokenType.Keyword, "") },
        { "ONLY", (TokenType.Keyword, "") },
        { "OPAQUE", (TokenType.Keyword, "") },
        { "OPEN", (TokenType.Keyword, "") },
        { "OPERATOR", (TokenType.Keyword, "") },
        { "ORACLE", (TokenType.Keyword, "") },
        { "ORADATA", (TokenType.Keyword, "") },
        { "ORGANIZATION", (TokenType.Keyword, "") },
        { "ORLANY", (TokenType.Keyword, "") },
        { "ORLVARY", (TokenType.Keyword, "") },
        { "OTHERS", (TokenType.Keyword, "") },
        { "OUT", (TokenType.Keyword, "") },
        { "OVERRIDING", (TokenType.Keyword, "") },
        { "PACKAGE", (TokenType.Keyword, "") },
        { "PARALLEL_ENABLE", (TokenType.Keyword, "") },
        { "PARAMETER", (TokenType.Keyword, "") },
        { "PARAMETERS", (TokenType.Keyword, "") },
        { "PARENT", (TokenType.Keyword, "") },
        { "PARTITION", (TokenType.Keyword, "") },
        { "PASCAL", (TokenType.Keyword, "") },
        { "PERSISTABLE", (TokenType.Keyword, "") },
        { "PIPE", (TokenType.Keyword, "") },
        { "PIPELINED", (TokenType.Keyword, "") },
        { "PLUGGABLE", (TokenType.Keyword, "") },
        { "POLYMORPHIC", (TokenType.Keyword, "") },
        { "PRAGMA", (TokenType.Keyword, "") },
        { "PRECISION", (TokenType.Keyword, "") },
        { "PRIOR", (TokenType.Keyword, "") },
        { "PRIVATE", (TokenType.Keyword, "") },
        { "RAISE", (TokenType.Keyword, "") },
        { "RANGE", (TokenType.Keyword, "") },
        { "RAW", (TokenType.Keyword, "") },
        { "READ", (TokenType.Keyword, "") },
        { "RECORD", (TokenType.Keyword, "") },
        { "REF", (TokenType.Keyword, "") },
        { "REFERENCE", (TokenType.Keyword, "") },
        { "RELIES_ON", (TokenType.Keyword, "") },
        { "REM", (TokenType.Keyword, "") },
        { "REMAINDER", (TokenType.Keyword, "") },
        { "RENAME", (TokenType.Keyword, "") },
        { "RESULT", (TokenType.Keyword, "") },
        { "RESULT_CACHE", (TokenType.Keyword, "") },
        { "RETURN", (TokenType.Keyword, "") },
        { "RETURNING", (TokenType.Keyword, "") },
        { "REVERSE", (TokenType.Keyword, "") },
        { "ROLLBACK", (TokenType.Keyword, "") },
        { "ROW", (TokenType.Keyword, "") },
        { "SAMPLE", (TokenType.Keyword, "") },
        { "SAVE", (TokenType.Keyword, "") },
        { "SAVEPOINT", (TokenType.Keyword, "") },
        { "SB1", (TokenType.Keyword, "") },
        { "SB2", (TokenType.Keyword, "") },
        { "SB4", (TokenType.Keyword, "") },
        { "SECOND", (TokenType.Keyword, "") },
        { "SEGMENT", (TokenType.Keyword, "") },
        { "SELF", (TokenType.Keyword, "") },
        { "SEPARATE", (TokenType.Keyword, "") },
        { "SEQUENCE", (TokenType.Keyword, "") },
        { "SERIALIZABLE", (TokenType.Keyword, "") },
        { "SET", (TokenType.Keyword, "") },
        { "SHORT", (TokenType.Keyword, "") },
        { "SIZE_T", (TokenType.Keyword, "") },
        { "SOME", (TokenType.Keyword, "") },
        { "SPARSE", (TokenType.Keyword, "") },
        { "SQLCODE", (TokenType.Keyword, "") },
        { "SQLDATA", (TokenType.Keyword, "") },
        { "SQLNAME", (TokenType.Keyword, "") },
        { "SQLSTATE", (TokenType.Keyword, "") },
        { "STANDARD", (TokenType.Keyword, "") },
        { "STATIC", (TokenType.Keyword, "") },
        { "STDDEV", (TokenType.Keyword, "") },
        { "STORED", (TokenType.Keyword, "") },
        { "STRING", (TokenType.Keyword, "") },
        { "STRUCT", (TokenType.Keyword, "") },
        { "STYLE", (TokenType.Keyword, "") },
        { "SUBMULTISET", (TokenType.Keyword, "") },
        { "SUBPARTITION", (TokenType.Keyword, "") },
        { "SUBSTITUTABLE", (TokenType.Keyword, "") },
        { "SUM", (TokenType.Keyword, "") },
        { "SYNONYM", (TokenType.Keyword, "") },
        { "TDO", (TokenType.Keyword, "") },
        { "THE", (TokenType.Keyword, "") },
        { "TIME", (TokenType.Keyword, "") },
        { "TIMESTAMP", (TokenType.Keyword, "") },
        { "TIMEZONE_ABBR", (TokenType.Keyword, "") },
        { "TIMEZONE_HOUR", (TokenType.Keyword, "") },
        { "TIMEZONE_MINUTE", (TokenType.Keyword, "") },
        { "TIMEZONE_REGION", (TokenType.Keyword, "") },
        { "TRAILING", (TokenType.Keyword, "") },
        { "TRANSACTION", (TokenType.Keyword, "") },
        { "TRANSACTIONAL", (TokenType.Keyword, "") },
        { "TRUSTED", (TokenType.Keyword, "") },
        { "UB1", (TokenType.Keyword, "") },
        { "UB2", (TokenType.Keyword, "") },
        { "UB4", (TokenType.Keyword, "") },
        { "UNDER", (TokenType.Keyword, "") },
        { "UNPLUG", (TokenType.Keyword, "") },
        { "UNSIGNED", (TokenType.Keyword, "") },
        { "UNTRUSTED", (TokenType.Keyword, "") },
        { "USE", (TokenType.Keyword, "") },
        { "USING", (TokenType.Keyword, "") },
        { "VALIST", (TokenType.Keyword, "") },
        { "VALUE", (TokenType.Keyword, "") },
        { "VARIABLE", (TokenType.Keyword, "") },
        { "VARIANCE", (TokenType.Keyword, "") },
        { "VARRAY", (TokenType.Keyword, "") },
        { "VARYING", (TokenType.Keyword, "") },
        { "VOID", (TokenType.Keyword, "") },
        { "WHILE", (TokenType.Keyword, "") },
        { "WORK", (TokenType.Keyword, "") },
        { "WRAPPED", (TokenType.Keyword, "") },
        { "WRITE", (TokenType.Keyword, "") },
        { "YEAR", (TokenType.Keyword, "") },
        { "ZONE", (TokenType.Keyword, "") },
        //
        { "TRUE", (TokenType.Value, "boolean") },
        { "FALSE", (TokenType.Value, "boolean") },
    };

    Token? InlineComment(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "-");

        if ('-' != source[pos.next])
        {
            return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "-");
        }

        var buffer = new StringBuilder();
        var start = pos.Clone();
        for (pos += 2; pos < source.Length; pos++)
        {
            var c = source[pos];
            if ('\r' == c) continue;
            if ('\n' == c)
            {
                break;
            }
            buffer.Append(c);
        }
        return new Token(TokenType.Commentary, "inline", start.line, start.col, start.pos, buffer.ToString());
    }

    Token? MultilineComment(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "/");

        if ('*' != source[pos.next])
        {
            return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "/");
        }

        var buffer = new StringBuilder();
        var start = pos.Clone();
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
                    return new Token(TokenType.Commentary, "multiline", start.line, start.col, start.pos, buffer.ToString());
                }
            }
            buffer.Append(c);
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unclosed commentary");
    }

    Token? LessOrEqual(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "<");
        if ('=' == source[pos.next])
        {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "<=");
        }
        if ('>' == source[pos.next])
        {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "<>");
        }
        if ('<' == source[pos.next])
        {
            var start = pos.Clone();
            pos += 2;
            var token = Identifier(pos, source);
            if (null == token) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "identifier expected");
            if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, ">> expected");
            if (pos.next + 1 >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, ">> expected");
            if (">>" != source.Substring(pos.next, 2)) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, ">> expected");
            token.Kind = "label";
            token.Col = start.col;
            token.Pos = start.pos;
            pos += 2;
            return token;
        }
        return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "<");
    }

    Token? Exponential(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "*");
        if ('*' == source[pos.next])
        {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "**");
        }
        return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, "*");
    }

    Token? GreaterOrEqual(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, ">");
        if ('=' == source[pos.next])
        {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, ">=");
        }
        return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, ">");
    }

    Token? NotEqual(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character !");
        if ('=' == source[pos.next])
        {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "!=");
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character !");
    }

    Token? Assign(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character :");
        if ('=' == source[pos.next])
        {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, ":=");
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character :");
    }

    Token? Concat(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character |");
        if ('|' == source[pos.next])
        {
            pos++;
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "||");
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character |");
    }

    Token? KeywordOrIdentifier(LexerPosition pos, string value)
    {
        var key = value.ToUpper();
        var (type, kind) = Keywords.ContainsKey(key) ?
            Keywords[key] :
            (TokenType.Name, "case insensetive");
        return new Token(type, kind, pos.line, pos.col, pos.pos, value);
    }

    Token? Identifier(LexerPosition pos, string source)
    {
        var start = pos.Clone();
        var sb = new StringBuilder();
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if (char.IsWhiteSpace(c)) return KeywordOrIdentifier(start, sb.ToString());
            if (!char.IsAsciiLetterOrDigit(c) && '_' != c && '#' != c && '$' != c)
            {
                pos--;
                return KeywordOrIdentifier(start, sb.ToString());
            }
            sb.Append(c);
        }
        return KeywordOrIdentifier(start, sb.ToString());
    }

    Token? Range(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, ".");
        if ('.' == source[pos.next])
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "..");
        if (char.IsDigit(source[pos.next]))
        {
            return Number(pos, source);
        }
        return new Token(TokenType.Operator, pos.line, pos.col, pos.pos, ".");
    }

    Token? Relational1(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character ~");
        if ('~' == source[pos.next])
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "~=");
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character ~");
    }

    Token? Relational2(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character ^");
        if ('=' == source[pos.next])
            return new Token(TokenType.Operator, pos.line, pos.col - 1, pos.pos - 1, "^=");
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unknown character ^");
    }

    Token? Literal(LexerPosition pos, string source)
    {
        var sb = new StringBuilder();
        var start = pos.Clone();
        pos++;
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if ('\'' == c)
            {
                if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unclosed literal");
                if ('\'' == source[pos.next]) pos++;
                else return new Token(TokenType.Value, "literal", start.line, start.col, start.pos, sb.ToString());
            }
            sb.Append(c);
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unclosed literal");
    }

    Token? CaseSensetiveIdentifier(LexerPosition pos, string source)
    {
        var sb = new StringBuilder();
        var start = pos.Clone();
        pos++;
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if ('\"' == c)
            {
                return new Token(TokenType.Name, "case sensetive", start.line, start.col, start.pos, sb.ToString());
            }
            sb.Append(c);
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unclosed case sensetive identifier");
    }

    Token? Number(LexerPosition pos, string source)
    {
        var sb = new StringBuilder();
        var start = pos.Clone();
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if (char.IsWhiteSpace(c)) return new Token(TokenType.Value, "number", start.line, start.col, start.pos, sb.ToString());
            if ('e' == c || 'E' == c)
            {
                if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unclosed exponent");
                sb.Append(c);
                pos++;
                c = source[pos];
                if (!char.IsDigit(c) && '.' != c && '-' != c && '+' != c)
                {
                    pos--;
                    return new Token(TokenType.Value, "number", start.line, start.col, start.pos, sb.ToString());
                }
                sb.Append(c);
                continue;
            }
            if (!char.IsDigit(c) && '.' != c)
            {
                if ('f' == c || 'F' == c || 'd' == c || 'D' == c)
                {
                    sb.Append(c);
                }
                else
                {
                    pos--;
                }
                return new Token(TokenType.Value, "number", start.line, start.col, start.pos, sb.ToString());
            }
            sb.Append(c);
        }
        return new Token(TokenType.Value, "number", start.line, start.col, start.pos, sb.ToString());
    }

    Token? QLiteral(LexerPosition pos, string source)
    {
        if (pos.next >= source.Length) return new Token(TokenType.Name, pos.line, pos.col, pos.pos, $"{source[pos]}");
        if ('\'' != source[pos.next])
            return Identifier(pos, source);
        var start = pos.Clone();
        pos++;
        if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, $"unclosed literal");
        pos++;
        var begin = source[pos];
        var end = begin switch 
        {  
            '!' => '!',
            '[' => ']',
            '{' => '}',
            '(' => ')',
            '<' => '>',
            _ => '\0'
        };
        if ('\0' == end) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "bad literal start, expected !, [, {, (, < ");
        pos++;

        var sb = new StringBuilder();
        for (; pos < source.Length; pos++)
        {
            var c = source[pos];
            if (end == c)
            {
                if (pos.next >= source.Length) return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unclosed literal");
                if ('\'' == source[pos.next]) {
                    pos++;
                    return new Token(TokenType.Value, "literal q", start.line, start.col, start.pos, sb.ToString());
                }
            }
            sb.Append(c);
        }
        return new Token(TokenType.Error, pos.line, pos.col, pos.pos, "unclosed literal");
    }

    Token? NQLiteral(LexerPosition pos, string source) {
        var c = source[pos];
        if (pos.next >= source.Length) return new Token(TokenType.Name, pos.line, pos.col, pos.pos, $"{c}");
        if ('q' != source[pos.next] && 'Q' != source[pos.next])
            return Identifier(pos, source);
        var start = pos.Clone();
        pos++;
        var token = QLiteral(pos, source);
        if (null == token) return new Token(TokenType.Name, pos.line, pos.col, pos.pos, $"{c}");
        if (TokenType.Name == token.Type) token.Text = $"n{token.Text}";
        else if (TokenType.Value == token.Type) {
            token.Kind = "literal nq";
            token.Col = start.col;
            token.Pos = start.pos;
        }
        return token;
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
                    token = new Token(TokenType.Error, pos.line, pos.col, pos.pos, $"unknown character {c}");
                    break;
                // literal
                case '\'':
                    token = Literal(pos, source);
                    break;
                case 'q':
                case 'Q':
                    token = QLiteral(pos, source);
                    break;
                case 'n':
                case 'N':
                    token = NQLiteral(pos, source);
                    break;
                // case sensetive
                case '"':
                    token = CaseSensetiveIdentifier(pos, source);
                    break;
                // identifier part
                case '#':
                case '$':
                case '_':
                    token = new Token(TokenType.Error, pos.line, pos.col, pos.pos, $"unknown character {c}");
                    break;
                default:
                    if (char.IsWhiteSpace(c)) continue;
                    if (char.IsAsciiLetter(c)) token = Identifier(pos, source);
                    else if (char.IsDigit(c)) token = Number(pos, source);
                    else token = new Token(TokenType.Error, pos.line, pos.col, pos.pos, $"unknown character {c}");
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