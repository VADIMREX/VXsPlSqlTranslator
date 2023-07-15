using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary>Anonymous block DECLARE ... </summary>
public class PlSqlAnonymousBlock : AstNodeParser
{
    // public string Name = "";

    public List<PlSqlTypeDeclaration> Types = new();

    public List<IAstNode> Cursors = new();

    public List<PlSqlVariable> Variables = new();

    public List<PlSqlProcedure> Procedures = new();

    public List<PlSqlFunction> Functions = new();

    public PlSqlBlockStatement? Block = null;

    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        if (TokenType.Keyword == token.Type)
        {
            switch (token.GetPlSqlText())
            {
                case "PROCEDURE":
                    Procedures.Add(AddChild(new PlSqlProcedure(enumerator)));
                    return (0, StateResult.Continue);
                case "FUNCTION":
                    Functions.Add(AddChild(new PlSqlFunction(enumerator)));
                    return (0, StateResult.Continue);
                case "TYPE":
#warning todo type
                    return (0, StateResult.Continue);
                case "CURSOR":
#warning todo cursor
                    return (0, StateResult.Continue);
                case "BEGIN":
                    Block = AddChild(new PlSqlBlockStatement(enumerator));
                    return (-1, StateResult.Return);
                default:
#warning todo error
                    return (-1, StateResult.Return);
            }
        }
        else if (TokenType.Name == token.Type)
        {
            Variables.Add(AddChild(new PlSqlVariable(enumerator)));
            #warning todo check on ";"
            return (0, StateResult.Continue);
        }
#warning todo error
        return (-1, StateResult.Return);
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
    }

    public PlSqlAnonymousBlock(IEnumerator<Token> enumerator) : base(enumerator, "anonymous block") { }
}