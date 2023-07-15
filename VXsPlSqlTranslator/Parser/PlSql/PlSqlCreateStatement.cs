using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary>SQL CREATE [OR REPLACE] (PACKAGE|PROCEDURE|FUNCTION|TYPE) statement.</summary>
public class PlSqlCreateStatement : AstNodeParser
{
    protected virtual (int, StateResult) State0PossibleOr(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if ("OR" == token.GetPlSqlText()) return (1, StateResult.Continue);
        return State2PackageProcedureFunctionType(enumerator);
    }

    protected virtual (int, StateResult) State1Replace(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if ("REPLACE" == token.GetPlSqlText())
        {
            WithOrReplace = true;
            return (2, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected virtual (int, StateResult) State2PackageProcedureFunctionType(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        switch (token.GetPlSqlText())
        {
            case "PACKAGE":
                Childs.Add(new PlSqlPackage(enumerator));
                break;
            case "PROCEDURE":
                Childs.Add(new PlSqlProcedure(enumerator));
                break;
            case "FUNCTION":
                Childs.Add(new PlSqlFunction(enumerator));
                break;
            case "TYPE":
                Childs.Add(new PlSqlFunction(enumerator));
                break;
            default:
#warning todo error
                return (-1, StateResult.Return);
        }
        return (-1, StateResult.Return);
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0PossibleOr);
        stateActions.Add(State1Replace);
        stateActions.Add(State2PackageProcedureFunctionType);
    }

    public bool WithOrReplace;
    public PlSqlCreateStatement(IEnumerator<Token> enumerator) : base(enumerator, "create") { }
}