using VXs.Lexer;

namespace VXs.Parser.PlSql;

/// <summary></summary>
public class PlSqlBlockStatement : AstNodeParser
{
    protected TokenType EndType;
    protected string EndText;

    protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator)
    {
        var token = enumerator.Current;
        switch (token.Type)
        {
            case TokenType.Keyword:
                switch (token.GetPlSqlText())
                {
                    case "NULL":
                        AddChild(new AstNode(token, "null"));
                        break;
                    case "IF":
                        AddChild(new PlSqlIfStatement(enumerator));
                        break;
                    case "CASE":
                        break;
                    case "LOOP":
                        AddChild(new PlSqlLoop(enumerator));
                        break;
                    case "FOR":
                        AddChild(new PlSqlFor(enumerator));
                        break;
                    case "WHILE":
                        break;
                    case "DECLARE":
                        AddChild(new PlSqlAnonymousBlock(enumerator));
                        break;
                    case "BEGIN":
                        AddChild(new PlSqlBlockStatement(enumerator));
                        break;
                    case "SELECT":
                        AddChild(new PlSqlSelect(enumerator));
                        break;
                    case "WITH":
                        break;
                    case "INSERT":
                        break;
                    case "UPDATE":
                        break;
                    case "MERGE":
                        break;
                    case "DELETE":
                        break;
                    case "EXECUTE":
                        break;
                    case "EXCEPTION":
                        break;
                    case "ELSIF":
                    case "ELSE":
                        return (-1, StateResult.Return);
                    case "END":
                        End(enumerator);
                        enumerator.MoveNext();
                        return (-1, StateResult.Return);
                    default:
                        AddChild(new PlSqlExpression(enumerator));
                        break;
                }
                break;
            case TokenType.Name:
                AddChild(new PlSqlExpression(enumerator));
                break;
        }
        return (0, StateResult.Continue);
    }

    protected void End(IEnumerator<Token> enumerator)
    {
        while (enumerator.MoveNext())
        {
            var token = enumerator.Current;
            if (TokenType.Commentary == token.Type)
            {
                continue;
            }
            if (TokenType.Error == token.Type)
            {
                continue;
            }
            if (TokenType.Name == token.Type)
            {
                continue;
            }
            if (TokenType.Special == token.Type)
            {
                return;
            }
        }
    }

    protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

    protected override void InitStates()
    {
        stateActions.Add(State0);
    }
    public PlSqlBlockStatement(IEnumerator<Token> enumerator, TokenType endType = TokenType.None, string endText = "") : base(enumerator.Current, "block")
    {
        EndType = endType;
        EndText = endText;
        InitStates();
        Parse(enumerator);
    }
}