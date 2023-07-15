using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlSelect : AstNodeParser
{
    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        switch (token.Type)
        {
            case TokenType.Keyword:
                switch (token.GetPlSqlText())
                {
                }
                break;
        }
        return (-1, StateResult.Return);
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
    }

    public PlSqlSelect(IEnumerator<Token> enumerator) : base(enumerator, "select") { }
}