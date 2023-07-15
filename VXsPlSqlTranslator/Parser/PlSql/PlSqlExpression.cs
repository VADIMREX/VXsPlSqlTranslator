using VXs.Lexer;

namespace VXs.Parser.PlSql;
/// <summary></summary>
public class PlSqlExpression : AstNodeParser
{
    protected TokenType EndType;
    protected string EndText;

    protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        switch (token.Type)
        {
            case TokenType.Keyword:
                goto case TokenType.Name;
            case TokenType.Name:
                AddChild(new PlSqlName(enumerator));
                return State1(enumerator);
            case TokenType.Value:
                AddChild(new AstNode(token, "value"));
                return (1, StateResult.Continue);
            case TokenType.Special:
                if ("(" == token.Text)
                {
                    return (0, StateResult.Continue);
                }
                goto case TokenType.Operator;
            case TokenType.Operator:
                return State1(enumerator);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (EndType == token.Type)
        {
            var text = TokenType.Keyword == token.Type ? token.GetPlSqlText() : token.Text;
            if (EndText == text)
                return (-1, StateResult.Return);
        }
        switch (token.Type)
        {
            case TokenType.Keyword:
                if ("END" == token.GetPlSqlText())
                    return (-1, StateResult.Return);
                goto case TokenType.Operator;
            case TokenType.Special:
                switch (token.Text)
                {
                    case "(":
                        AddChild(new PlSqlExpression(enumerator));
                        return (0, StateResult.Continue);
                    case ")":
                        return (-1, StateResult.Return);
                    case ",":
                        AddChild(new AstNode(token, "operator"));
                        return (0, StateResult.Continue);
                    case ";":
                        return (-1, StateResult.Return);
                }
#warning todo error
                return (-1, StateResult.Return);
            case TokenType.Operator:
                AddChild(new AstNode(token, "operator"));
                return (0, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
    }

    public PlSqlExpression(IEnumerator<Token> enumerator, TokenType endType = TokenType.None, string endText = "") : base(enumerator.Current, "expression")
    {
        EndType = endType;
        EndText = endText;
        InitStates();
        Parse(enumerator);
    }
}