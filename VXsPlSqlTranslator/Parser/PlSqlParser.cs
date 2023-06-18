namespace VXs.Parser;

using VXs.Lexer;

public class PlSqlParser {
    class PlSqlCreate : AstNode {
        public bool WithOrReplace;
        public PlSqlCreate(IEnumerator<Token> enumerator) : base(enumerator.Current, "create") {
            int state = 0;
            while (enumerator.MoveNext()) {
                var token = enumerator.Current;
                if (TokenType.Commentary == token.Type) {
                    continue;
                }
                if (TokenType.Error == token.Type) {
                    continue;
                }
                if (TokenType.Keyword != token.Type) {
                    #warning todo error
                    continue;
                }
                switch (state) {
                    case 0:
                        if ("OR" == token.GetPlSqlText()) {
                            state = 1;
                            continue;
                        }
                        state = 2;
                        goto case 2;
                    case 1:
                        if ("REPLACE" == token.GetPlSqlText()) {
                            WithOrReplace = true;
                            state = 2;
                            continue;
                        }
                        #warning todo error
                        return;
                    case 2:
                        switch(token.GetPlSqlText()) {
                            case "PACKAGE":
                                Childs.Add(new PlSqlPackage(enumerator));
                                break;
                            case "PROCEDURE":
                                Childs.Add(new PlSqlProcedure(enumerator));
                                break;
                            case "FUNCTION":
                                Childs.Add(new PlSqlFunction(enumerator));
                                break;
                            default:
                                #warning todo error
                                break;
                        }
                        return;
                }
            }
        }
    }

    class PlSqlPackage : AstNode {
        public bool IsBody = false;
        public string Name = "";
        public PlSqlPackage(IEnumerator<Token> enumerator) : base(enumerator.Current, "package") {
            int state = 0;
            while (enumerator.MoveNext()) {
                var token = enumerator.Current;
                if (TokenType.Commentary == token.Type) {
                    continue;
                }
                if (TokenType.Error == token.Type) {
                    continue;
                }
                switch(state) {
                    case 0:
                        if (TokenType.Keyword == token.Type) {
                            if ("BODY" == token.GetPlSqlText()) {
                                IsBody = true;
                                state = 1;
                                continue;
                            }
                            #warning todo error
                            return;
                        }
                        if (TokenType.Name == token.Type) {
                            state = 1;
                            goto case 1;
                        }
                        #warning todo error
                        return;
                    case 1:
                        if (TokenType.Name == token.Type) {
                            Name = token.Text;
                            state = 2;
                            continue;
                        }
                        #warning todo error
                        return;
                    case 2:
                        if (TokenType.Keyword == token.Type) {
                            if ("IS" == token.GetPlSqlText()) {
                                IsBody = true;
                                state = 3;
                                continue;
                            }
                            #warning todo error
                            return;
                        }
                        #warning todo error
                        return;
                    case 3:
                        if (TokenType.Keyword == token.Type) {
                            switch(token.GetPlSqlText()) {
                                case "PROCEDURE":
                                    AddChild(new PlSqlProcedure(enumerator));
                                    continue;
                                case "FUNCTION":
                                    AddChild(new PlSqlFunction(enumerator));
                                    continue;
                                case "TYPE":
                                    #warning todo type
                                    continue;
                                case "CURSOR":
                                    #warning todo cursor
                                    continue;
                                case "BEGIN":
                                    #warning todo block
                                    return;
                                case "END":
                                    #warning todo block
                                    return;
                                default:
                                    #warning todo error
                                    return;
                            }
                        }
                        else if (TokenType.Name == Token.Type) {
                            AddChild(new PlSqlVariable(enumerator));
                            continue;
                        }
                        #warning todo error
                        return;
                }
            }
        }
    }

    class PlSqlProcedure : AstNode {
        public string Name = "";

        public PlSqlProcedure(IEnumerator<Token> enumerator) : base(enumerator.Current, "procedure") {
            int state = 0;
            while (enumerator.MoveNext()) {
                var token = enumerator.Current;
                if (TokenType.Commentary == token.Type) {
                    continue;
                }
                if (TokenType.Error == token.Type) {
                    continue;
                }
                switch(state) {
                    case 0:
                        if (TokenType.Name == token.Type) {
                            Name = token.Text;
                            state = 1;
                            continue;
                        }
                        #warning todo error
                        return;
                    case 1:
                        if (TokenType.Special == token.Type) {
                            switch(token.Text) {
                                case "(":
                                    state = 2;
                                    continue;
                                case ";":
                                    return;
                                default:
                                    #warning todo error
                                    return;
                            }
                        }
                        if (TokenType.Keyword == token.Type) {
                            state = 3;
                            goto case 3;
                        }
                        #warning todo error
                        return;
                    case 2:
                        AddChild(new PlSqlArgument(enumerator));
                        token = enumerator.Current;
                        if (TokenType.Special == token.Type) {
                            switch(token.Text) {
                                case ")":
                                    state = 3;
                                    continue;
                                case ";":
                                    return;
                            }
                            continue;
                        }
                        #warning todo error
                        return;
                    case 3:
                        if (TokenType.Keyword == token.Type) {
                            switch(token.GetPlSqlText()) {
                                case "IS":
                                    state = 4;
                                    continue;
                                case "AS":
                                    state = 5;
                                    continue;
                                default:
                                    #warning todo error
                                    return;
                            }
                        }
                        #warning todo error
                        return;
                    case 4:
                        if (TokenType.Keyword == token.Type) {
                            switch(token.GetPlSqlText()) {
                                case "PROCEDURE":
                                    AddChild(new PlSqlProcedure(enumerator));
                                    continue;
                                case "FUNCTION":
                                    AddChild(new PlSqlFunction(enumerator));
                                    continue;
                                case "TYPE":
                                    #warning todo type
                                    continue;
                                case "CURSOR":
                                    #warning todo cursor
                                    continue;
                                case "BEGIN":
                                    #warning todo block
                                    return;
                                default:
                                    #warning todo error
                                    return;
                            }
                        }
                        else if (TokenType.Name == Token.Type) {
                            AddChild(new PlSqlVariable(enumerator));
                            continue;
                        }
                        #warning todo error
                        return;
                    case 5:
                        #warning todo external procedure
                        return;
                }
            }
        }
    }

    class PlSqlFunction : AstNode {
        public PlSqlFunction(IEnumerator<Token> enumerator) : base(enumerator.Current, "function") {
            int state = 0;
            while (enumerator.MoveNext()) {
            }
        }
    }

    class PlSqlAnonymousBlock : AstNode {
        public PlSqlAnonymousBlock(IEnumerator<Token> enumerator) : base(enumerator.Current, "anonymous block") {
            int state = 0;
            while (enumerator.MoveNext()) {
            }
        }
    }

    class PlSqlBlock : AstNode {
        public PlSqlBlock(IEnumerator<Token> enumerator) : base(enumerator.Current, "block") {
            int state = 0;
            while (enumerator.MoveNext()) {
            }
        }
    }

    class PlSqlVariable: AstNode {
        public PlSqlVariable(IEnumerator<Token> enumerator) : base(enumerator.Current, "variable") {
            int state = 0;
            while (enumerator.MoveNext()) {
            }
        }
    }

    class PlSqlArgument: AstNode {
        public PlSqlArgument(IEnumerator<Token> enumerator) : base(enumerator.Current, "variable") {
            int state = 0;
            while (enumerator.MoveNext()) {
            }
        }
    }

    public IAstNode Parse(IEnumerable<Token> tokens) {
        var res = new AstNode(Token.None, "root");
        var enumerator = tokens.GetEnumerator();
        while (enumerator.MoveNext()) {
            var token = enumerator.Current;
            if (TokenType.Commentary == token.Type) {
                continue;
            }
            if (TokenType.Error == token.Type) {
                continue;
            }
            if (TokenType.Keyword != token.Type) {
                continue;
            }
            switch(token.GetPlSqlText()) {
                case "CREATE":
                    res.AddChild(new PlSqlCreate(enumerator));
                    break;
                case "PACKAGE":
                    res.AddChild(new PlSqlPackage(enumerator));
                    break;
                case "PROCEDURE":
                    res.AddChild(new PlSqlProcedure(enumerator));
                    break;
                case "FUNCTION":
                    res.AddChild(new PlSqlFunction(enumerator));
                    break;
                case "DECLARE":
                    res.AddChild(new PlSqlAnonymousBlock(enumerator));
                    break;
                default:
                    break;
            }
        }
        return res;
    }
}