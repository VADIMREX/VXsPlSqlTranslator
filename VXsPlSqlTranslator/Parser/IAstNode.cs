namespace VXs.Parser;

using VXs.Lexer;

public interface IAstNode {
    Token StartToken { get; }
    string Type { get; }
    IAstNode? Parent { get; set; }
    List<IAstNode> Childs { get; }
    string ToString(int level);
}