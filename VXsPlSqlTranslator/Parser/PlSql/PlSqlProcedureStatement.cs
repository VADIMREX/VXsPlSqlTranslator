using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlProcedure : PlSqlAnonymousBlock
{
    public string Name = "";

    public bool IsExternal = false;

    public bool IsDeclaration = true;

    public List<PlSqlArgument> Arguments = new();

    protected override (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Name == token.Type)
        {
            Name = token.Text;
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
                case "(": return (2, StateResult.Continue);
                case ";": return (-1, StateResult.Return);
                default:
#warning todo error
                    return (-1, StateResult.Return);
            }
        }
        if (TokenType.Keyword == token.Type)
        {
            return State3(enumerator);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator)
    {
        Arguments.Add(AddChild(new PlSqlArgument(enumerator)));
        var token = enumerator.Current;
        if (TokenType.Special == token.Type)
        {
            switch (token.Text)
            {
                case ")": return (3, StateResult.Continue);
                case ";": return (-1, StateResult.Return);
                case ",": return (2, StateResult.Continue);
                default:
#warning todo error
                    return (-1, StateResult.Return);
            }
        }
        if (TokenType.Keyword == token.Type)
        {
            return (3, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State3(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Keyword == token.Type)
        {
            switch (token.GetPlSqlText())
            {
                case "IS": return (4, StateResult.Continue);
                case "AS": return (5, StateResult.Continue);
                default:
#warning todo error
                    return (-1, StateResult.Return);
            }
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State4(IEnumerator<Token> enumerator)
    {
        var (state, result) = base.State0(enumerator);
        if (0 == state) state = 4;
        return (state, result);
    }

    protected virtual (int, StateResult) State5(IEnumerator<Token> enumerator)
    {
#warning todo external procedure
        return (-1, StateResult.Return);
    }

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
        stateActions.Add(State2);
        stateActions.Add(State3);
        stateActions.Add(State4);
        stateActions.Add(State5);
    }

    public PlSqlProcedure(IEnumerator<Token> enumerator) : base(enumerator)
    {
        Type = "procedure";
    }
}