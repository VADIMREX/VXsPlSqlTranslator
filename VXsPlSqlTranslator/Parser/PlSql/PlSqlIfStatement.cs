using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlIfStatement : AstNodeParser
{
    public List<PlSqlExpression> Conditions = new();
    public List<PlSqlBlockStatement> Blocks = new();

    protected (PlSqlExpression? condition, AstNode actions) currentBlock;

    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        Conditions.Add(AddChild(new PlSqlExpression(enumerator, TokenType.Keyword, "THEN")));
        return (1, StateResult.Continue);
    }

    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
    {
        Blocks.Add(AddChild(new PlSqlBlockStatement(enumerator)));
        var token = enumerator.Current;
        return (0, StateResult.Continue);
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
        stateActions.Add(State1);
    }

    public PlSqlIfStatement(IEnumerator<Token> enumerator) : base(enumerator.Current, "if")
    {

    }
}
