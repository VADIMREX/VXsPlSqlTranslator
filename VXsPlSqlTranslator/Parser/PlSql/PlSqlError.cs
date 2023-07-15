using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
    public class PlSqlError : IAstNode
    {
        string message;

        Token startToken;
        public Token StartToken => startToken;

        public string Type => "Error";

        public IAstNode? Parent { get; set; }

        public List<IAstNode> Childs => throw new NotImplementedException();
        
        public PlSqlError(Token token, string message) {
            startToken = token;
            this.message = message;
        }

        public string ToString(int level) => $"{message}".PadLeft(message.Length + level * 4, ' ');
    }