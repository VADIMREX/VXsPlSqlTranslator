using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlType : AstNodeParser
{
    public IAstNode Name;
    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Name == token.Type || TokenType.Keyword == token.Type)
        {
            Name = AddChild(new PlSqlName(enumerator));
            token = enumerator.Current;
            if (TokenType.Special == token.Type)
            {
                if ("(" == token.Text)
                {
                    return (1, StateResult.Continue);
                }
            }
            return (-1, StateResult.Return);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Special == token.Type)
        {
            if (")" == token.Text)
            {
                enumerator.MoveNext();
                return (-1, StateResult.Return);
            }
            if ("," == token.Text)
            {
                return (1, StateResult.Continue);
            }
        }
        if (TokenType.Value == token.Type)
        {
            AddChild(new AstNode(token, "dimension"));
            return (1, StateResult.Continue);
        }
        if (TokenType.Name == token.Type)
        {
            AddChild(new PlSqlName(enumerator));
            return State1(enumerator);
        }
        if (TokenType.Keyword == token.Type)
        {
            AddChild(new PlSqlName(enumerator));
            return State1(enumerator);
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

    public PlSqlType(IEnumerator<Token> enumerator) : base(enumerator, "type") { }
}