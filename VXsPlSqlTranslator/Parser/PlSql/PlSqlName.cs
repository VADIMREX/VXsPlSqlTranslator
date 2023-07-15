using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlName : AstNodeParser
{
    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Name == token.Type)
        {
            AddChild(new AstNode(token, "name"));
            return (1, StateResult.Continue);
        }
        else if (TokenType.Keyword == token.Type)
        {
            AddChild(new AstNode(token, "name"));
            return (1, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Special == token.Type)
        {
            switch (token.Text)
            {
                case ";":
                case ",":
                case "(":
                case ")":
                    return (-1, StateResult.Return);
                case ".":
                    AddChild(new AstNode(token, "member"));
                    return (0, StateResult.Continue);
                case "%":
                    AddChild(new AstNode(token, "attribute"));
                    return (0, StateResult.Continue);
                case "@":
                    AddChild(new AstNode(token, "remote"));
                    return (0, StateResult.Continue);
                default:
#warning todo error
                    return (-1, StateResult.Return);
            }
        }
        else if (TokenType.Keyword == token.Type)
        {
            switch (token.GetPlSqlText())
            {
                case "AS":
                case "END":
                    return (-1, StateResult.Return);
            }
        }
#warning todo error
        return (-1, StateResult.Return);
    }
    protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
    }

    public PlSqlName(IEnumerator<Token> enumerator) : base(enumerator, "name") { }
}