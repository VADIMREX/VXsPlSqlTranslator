namespace VXs.Lexer;

public class PlSqlLexer {
    
    void Token InlineComment(IEnumerator<char> enumerator) {
        
    }
    
    public IEnumerable<Token> Parse(IEnumerable<char> source) {
        var pos = -1;
        var line = 0;
        var col = -1;
        var enumerator = source.GetEnumerator();
        
        while(enumerator.MoveNext()) {
            var c = enumerator.Current;
            switch(c) {
                case '+':
                case '*':
                    yield return new Token(TokenType.Operator, line, col, pos, $"{c}");
                    break;
                case '-':
                    ParseInlineComment(IEnumerator<char>)
            }
        }
    }
}