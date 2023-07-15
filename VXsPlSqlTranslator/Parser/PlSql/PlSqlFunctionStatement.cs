using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlFunction : PlSqlProcedure
{
    public IAstNode ReturnType;

    protected override (int, StateResult) State3(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Keyword == token.Type)
        {
            switch (token.GetPlSqlText())
            {
                case "RETURN": return (6, StateResult.Continue);
                default:
#warning todo error
                    return (-1, StateResult.Return);
            }
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State6(IEnumerator<Token> enumerator)
    {
        ReturnType = AddChild(new PlSqlType(enumerator));
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

    protected override void InitStates()
    {
        base.InitStates();
        stateActions.Add(State6);
    }

    public PlSqlFunction(IEnumerator<Token> enumerator) : base(enumerator)
    {
        Type = "function";
    }
}