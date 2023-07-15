using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary>PlSql PACKAGE or PACKAGE BODY</summary>
public class PlSqlPackage : PlSqlAnonymousBlock
{
    protected override (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Keyword == token.Type)
        {
            if ("BODY" == token.GetPlSqlText())
            {
                IsBody = true;
                return (1, StateResult.Continue);
            }
            return (-1, StateResult.Return);
        }
        if (TokenType.Name == token.Type) return State1(enumerator);
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Name == token.Type)
        {
            Name = token.Text;
            return (2, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Keyword == token.Type)
        {
            if ("IS" == token.GetPlSqlText())
            {
                return (3, StateResult.Continue);
            }
#warning todo error
            return (-1, StateResult.Return);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State3(IEnumerator<Token> enumerator)
    {
        var (state, result) = base.State0(enumerator);
        if (0 == state) state = 3;
        return (state, result);
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
        stateActions.Add(State2);
        stateActions.Add(State3);
    }

    public bool IsBody = false;
    public string Name = "";
    public PlSqlPackage(IEnumerator<Token> enumerator) : base(enumerator)
    {
        Type = "package";
    }
}