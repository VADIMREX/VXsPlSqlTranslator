using VXs.Lexer;

namespace VXs.Parser.PlSql;
/// <summary></summary>
public class PlSqlExpression : AstNodeParser
{
    protected TokenType EndType;
    protected string EndText;

    protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

    Token save;
    protected virtual (int, StateResult) ParseKeyword(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        switch(token.GetPlSqlText()) {
            case "NULL":
                AddChild(new AstNode(token, "null"));
                return (0, StateResult.Continue);
            case "IN":
                AddChild(new AstNode(token, "operator"));
                return (0, StateResult.Continue);
            case "IS":
                AddChild(new AstNode(token, "operator"));
                return (1, StateResult.Continue);
            default:
                return (0, StateResult.Continue);
        }
    }

    protected virtual (int, StateResult) ParseName(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        AddChild(new AstNode(token, "name"));
        return (0, StateResult.Continue);
    }

    protected virtual (int, StateResult) ParseValue(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        AddChild(new AstNode(token, "value"));
        return (0, StateResult.Continue);
    }

    protected virtual (int, StateResult) ParseSpecial(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        switch(token.GetPlSqlText()) {
            case ";":
                // is need to add to tree?
                return (-1, StateResult.Return);
            case "(":
                enumerator.MoveNext();
                AddChild(new PlSqlExpression(enumerator, TokenType.Special, ")"));
                return (-1, StateResult.Return);
            case ")":
                AddChild(new AstNode(token, "null"));
                return (-1, StateResult.Return);
            default:
                return (0, StateResult.Continue);
        }
    }

    protected virtual (int, StateResult) ParseOperator(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        AddChild(new AstNode(token, "operator"));
        return (0, StateResult.Continue);
    }
    
    /// <summary><summary>
    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        var state = -1;
        var stateResult = StateResult.Return;
        if (EndType == token.Type && EndText == token.GetPlSqlText())
        {
            return (state, stateResult);
        }
        switch (token.Type)
        {
            case TokenType.Keyword:
                (state, stateResult) = ParseKeyword(enumerator);
                break;
            case TokenType.Name:
                (state, stateResult) = ParseName(enumerator);
                break;
            case TokenType.Value:
                (state, stateResult) = ParseValue(enumerator);
                break;
            case TokenType.Special:
                (state, stateResult) = ParseSpecial(enumerator);
                break;
            case TokenType.Operator:
                (state, stateResult) = ParseOperator(enumerator);
                break;
        }
        return (state, stateResult);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        if (TokenType.Keyword != token.Type) {
            #warning error
            return (-1, StateResult.Return);
        }
        switch(token.GetPlSqlText()) {
            case "NULL":
                AddChild(new AstNode(token, "null"));
                return (0, StateResult.Continue);
            case "NOT":
                ((AstNode)(Childs[Childs.Count - 1])).AddChild(new AstNode(token, "operator"));
                return (2, StateResult.Continue);
            default:
                #warning error
                return (-1, StateResult.Return);
        }
    }

    protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator) {
        var token = enumerator.Current;
        if (TokenType.Keyword != token.Type) {
            #warning error
            return (-1, StateResult.Return);
        }
        if ("NULL" != token.GetPlSqlText()) {
            #warning error
            return (-1, StateResult.Return);
        }
        AddChild(new AstNode(token, "null"));
        return (0, StateResult.Continue);            
    }

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
        stateActions.Add(State2);
    }

    public PlSqlExpression(IEnumerator<Token> enumerator, TokenType endType = TokenType.None, string endText = "") : base(enumerator.Current, "expression")
    {
        EndType = endType;
        EndText = endText;
        InitStates();
        Parse(enumerator);
    }
}