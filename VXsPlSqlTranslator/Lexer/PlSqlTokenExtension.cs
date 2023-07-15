namespace VXs.Lexer;

public static class PlsSqlTokenExtension {
    public static string GetPlSqlText(this Token self) {
        switch (self.Type) {
            case TokenType.Keyword: 
            case TokenType.Operator:
                return self.Text.ToUpper();
            case TokenType.Name:
                if ("case insensetive" == self.Kind) return self.Text.ToUpper();
                if ("label" == self.Kind) return self.Text.ToUpper();
                return self.Text;
            case TokenType.Value:
                if ("null" == self.Kind) return "NULL";
                if ("boolean" == self.Kind) return self.Text.ToUpper();
                return self.Text;
        }
        return self.Text;
    }
}