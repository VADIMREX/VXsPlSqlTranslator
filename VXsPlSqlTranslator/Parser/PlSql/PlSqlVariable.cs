using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlVariable : AstNodeParser
{
    public string Name;
    public IAstNode VariableType;
    public IAstNode? Default = null;

    /// <summary>Name of variable</summary>
    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Name == token.Type)
        {
            AddChild(new AstNode(token, "name"));
            Name = token.Text;
            return (1, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    /// <summary>Type of Variable</summary>
    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        VariableType = AddChild(new PlSqlType(enumerator));
        var token = enumerator.Current;
        if (TokenType.Special == token.Type)
        {
            return (-1, StateResult.Return);
        }
        if (TokenType.Operator == token.Type)
        {
            if (":=" == token.Text)
            {
                return (2, StateResult.Continue);
            }
        }
        if (TokenType.Keyword == token.Type)
        {
            if ("DEFAULT" == token.GetPlSqlText())
            {
                return (2, StateResult.Continue);
            }
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    /// <summary>Default value</summary>
    protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator)
    {
        Default = AddChild(new PlSqlExpression(enumerator, TokenType.Special, ";"));
        return (-1, StateResult.Return);
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
        stateActions.Add(State2);
    }

    public PlSqlVariable(IEnumerator<Token> enumerator) : base(enumerator, "variable") { }
}