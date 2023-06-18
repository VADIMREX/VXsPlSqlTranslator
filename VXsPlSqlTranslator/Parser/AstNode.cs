namespace VXs.Parser;

using System.Text;
using VXs.Lexer;

public class AstNode : IAstNode {
    public Token Token { get; set; }
    public string Type { get; set; }

    public IAstNode? Parent { get; set; } = null;
    public List<IAstNode> Childs { get; set; } = new List<IAstNode>();

    public AstNode(Token token, string value) {
        Token = token;
        Type = value;
    }

    public AstNode(Token token, string value, IAstNode parent) : this(token, value) {
        Parent = parent;
    }

    public IAstNode AddChild(IAstNode child) {
        child.Parent = this;
        Childs.Add(child);
        return child;
    }
    
    public string ToString(int level) {
        var sb = new StringBuilder();    
        sb.AppendLine($"{new string(' ', level * 4)}{Token}: {Type}");
        foreach(var node in Childs)
            sb.AppendLine(node.ToString(level + 1));
        return sb.ToString();
    }

    public override string ToString() => ToString(0);
}