namespace VXs.Parser;

using System.Text;
using VXs.Lexer;

public class AstNode : IAstNode
{

    public Token StartToken { get; set; }
    public string Type { get; set; }

    public IAstNode? Parent { get; set; } = null;
    public List<IAstNode> Childs { get; set; } = new List<IAstNode>();

    public AstNode(Token token, string value)
    {
        StartToken = token;
        Type = value;
    }

    public AstNode(Token token, string value, IAstNode parent) : this(token, value)
    {
        Parent = parent;
    }

    public T AddChild<T>(T child) where T : IAstNode
    {
        child.Parent = this;
        Childs.Add(child);
        return child;
    }

    public string ToString(int level)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"{new string(' ', level * 4)}{StartToken}: {Type}");
        foreach (var node in Childs)
            sb.Append(node.ToString(level + 1));
        return sb.ToString();
    }

    public override string ToString() => ToString(0);
}

public enum StateResult
{
    Return,
    Continue
}

public abstract class AstNodeParser : AstNode
{
    protected List<Func<IEnumerator<Token>, (int, StateResult)>> stateActions = new();

    protected abstract void InitStates();

    protected abstract void Parse(IEnumerator<Token> enumerator);

    /// <summary> Get next token and parse them </summary>
    protected virtual void ParseNext(IEnumerator<Token> enumerator)
    {
        int state = 0;
        StateResult result = StateResult.Continue;
        while (enumerator.MoveNext())
        {
            var token = enumerator.Current;
            if (TokenType.Commentary == token.Type)
            {
                continue;
            }
            if (TokenType.Error == token.Type)
            {
                continue;
            }
            (state, result) = stateActions[state](enumerator);
            if (StateResult.Continue == result) continue;
            if (StateResult.Return == result) return;
        }
    }

    /// <summary> Parse current token first, and then move next </summary>
    protected virtual void ParseCurrent(IEnumerator<Token> enumerator)
    {
        int state = 0;
        StateResult result = StateResult.Continue;
        do
        {
            var token = enumerator.Current;
            if (TokenType.Commentary == token.Type)
            {
                continue;
            }
            if (TokenType.Error == token.Type)
            {
                continue;
            }
            (state, result) = stateActions[state](enumerator);
            if (StateResult.Continue == result) continue;
            if (StateResult.Return == result) return;
        } while (enumerator.MoveNext());
    }

    public AstNodeParser(Token token, string type) : base(token, type) {

    }

    public AstNodeParser(IEnumerator<Token> enumerator, string type) : base(enumerator.Current, type)
    {
        InitStates();
        Parse(enumerator);
    }
}