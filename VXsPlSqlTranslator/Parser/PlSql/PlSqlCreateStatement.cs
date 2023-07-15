using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary>SQL CREATE [OR REPLACE] (PACKAGE|PROCEDURE|FUNCTION|TYPE) statement.</summary>
public class PlSqlCreateStatement : AstNodeParser
{
    /// <summary>Prossible OR for OR REPLACE</summary>
    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if ("OR" == token.GetPlSqlText()) return (1, StateResult.Continue);
        return State2(enumerator);
    }

    /// <summary>REPLACE modifier</summary>
    protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator)
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
    
    /// <summary>PACKAGE PROCEDURE FUNCTION TYPE</summary>
    protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator)
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
                Childs.Add(new PlSqlTypeDeclaration(enumerator));
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
        stateActions.Add(State0);
        stateActions.Add(State1);
        stateActions.Add(State2);
    }

    public bool WithOrReplace;
    public PlSqlCreateStatement(IEnumerator<Token> enumerator) : base(enumerator, "create") { }
}