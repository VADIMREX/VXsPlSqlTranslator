using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlArgument : AstNodeParser
{
    public string Name;
    public bool IsIn = false;
    public bool IsOut = false;
    public IAstNode ArgumentType;
    public IAstNode? Default = null;

    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Name == token.Type)
        {
            Name = token.Text;
            return (1, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Keyword == token.Type)
        {
            if ("IN" == token.GetPlSqlText())
            {
                IsIn = true;
                return (1, StateResult.Continue);
            }
            if ("OUT" == token.GetPlSqlText())
            {
                IsOut = true;
                return (1, StateResult.Continue);
            }
        }
        return State2(enumerator);
    }

    protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator)
    {
        ArgumentType = AddChild(new PlSqlType(enumerator));
        var token = enumerator.Current;
        if (TokenType.Special == token.Type)
        {
            return (-1, StateResult.Return);
        }
        if (TokenType.Keyword == token.Type)
        {
            return State3(enumerator);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State3(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Keyword == token.Type)
        {
            if ("DEFAULT" == token.GetPlSqlText())
                return (4, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State4(IEnumerator<Token> enumerator)
    {
        Default = AddChild(new PlSqlExpression(enumerator, TokenType.Special, ","));
        return (-1, StateResult.Return);
    }


    protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
        stateActions.Add(State2);
        stateActions.Add(State3);
        stateActions.Add(State4);
    }

    public PlSqlArgument(IEnumerator<Token> enumerator) : base(enumerator, "argument") { }
}