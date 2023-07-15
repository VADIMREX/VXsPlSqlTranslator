using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlTypeDeclaration : AstNodeParser
{
    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
#warning todo error
        return (-1, StateResult.Return);
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
    }

    public PlSqlTypeDeclaration(IEnumerator<Token> enumerator) : base(enumerator, "type declaration") { }
}