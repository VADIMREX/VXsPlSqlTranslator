namespace VXs.Parser;

using VXs.Lexer;
using VXs.Parser.PlSql;

public class PlSqlParser {
   
    public IAstNode Parse(IEnumerable<Token> tokens) {
        var res = new AstNode(Token.None, "root");
        var enumerator = tokens.GetEnumerator();
        while (enumerator.MoveNext()) {
            var token = enumerator.Current;
            if (TokenType.Commentary == token.Type) {
                continue;
            }
            if (TokenType.Error == token.Type) {
                continue;
            }
            if (TokenType.Keyword != token.Type) {
                res.AddChild(new PlSqlError(token, "ORA-00900: invalid SQL statement"));
                continue;
            }
            switch(token.GetPlSqlText()) {
                // Хранимые сущности:
                case "CREATE":
                    res.AddChild(new PlSqlCreateStatement(enumerator));
                    break;
                case "PACKAGE":
                    res.AddChild(new PlSqlPackage(enumerator));
                    End(enumerator);
                    break;
                case "PROCEDURE":
                    res.AddChild(new PlSqlProcedure(enumerator));
                    break;
                case "FUNCTION":
                    res.AddChild(new PlSqlFunction(enumerator));
                    break;
                case "TYPE":
                    res.AddChild(new PlSqlTypeDeclaration(enumerator));
                    break;
                // Анонимные блоки
                case "DECLARE":
                    res.AddChild(new PlSqlAnonymousBlock(enumerator));
                    break;
                case "BEGIN":
                    res.AddChild(new PlSqlBlockStatement(enumerator));
                    break;
                // Обработка конца ???
                case "END":
                    End(enumerator);
                    break;                    
                default:
                    break;
            }
        }
        return res;
    }

    protected void End(IEnumerator<Token> enumerator) {
        while (enumerator.MoveNext()) {
            var token = enumerator.Current;
            if (TokenType.Commentary == token.Type) {
                continue;
            }
            if (TokenType.Error == token.Type) {
                continue;
            }
            if (TokenType.Name == token.Type) {
                continue;
            }
            if (TokenType.Special == token.Type) {
                return;
            }
        }
    }
}