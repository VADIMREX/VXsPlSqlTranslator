namespace VXs.Lexer;

public static class PlsSqlTokenExtension {
    public static string GetPlSqlText(this Token self) {
        if (TokenType.Keyword == self.Type) return self.Text.ToUpper();
        if (TokenType.Name == self.Type) {
            if ("case insensetive" == self.Kind) return self.Text.ToUpper();
            if ("label" == self.Kind) return self.Text.ToUpper();
        }
        return self.Text;
    }
}