namespace VXs.Parser;

using VXs.Lexer;

public class PlSqlParser {
    class PlSqlCreate : AstNodeParser {
        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if ("OR" == token.GetPlSqlText()) return (1, StateResult.Continue);
            return State2(enumerator);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if ("REPLACE" == token.GetPlSqlText()) {
                WithOrReplace = true;
                return (2, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
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
        public PlSqlCreate(IEnumerator<Token> enumerator) : base(enumerator, "create") { }
    }

    class PlSqlPackage : PlSqlAnonymousBlock {
        protected override (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Keyword == token.Type) {
                if ("BODY" == token.GetPlSqlText()) {
                    IsBody = true;
                    return (1, StateResult.Continue);
                }
                return (-1, StateResult.Return);
            }
            if (TokenType.Name == token.Type) return State1(enumerator);
            #warning todo error
            return (-1, StateResult.Return);
        }
        
        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Name == token.Type) {
                Name = token.Text;
                return (2, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Keyword == token.Type) {
                if ("IS" == token.GetPlSqlText()) {
                    return (3, StateResult.Continue);
                }
                #warning todo error
                return (-1, StateResult.Return);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State3(IEnumerator<Token> enumerator) {
            var (state, result) = base.State0(enumerator);
            if (0 == state) state = 3;
            return (state, result);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

        protected override void InitStates()
        {
            stateActions.Add(State0);
            stateActions.Add(State1);
            stateActions.Add(State2);
            stateActions.Add(State3);
        }

        public bool IsBody = false;
        public string Name = "";
        public PlSqlPackage(IEnumerator<Token> enumerator) : base(enumerator) {
            Type = "package";
        }
    }

    class PlSqlProcedure : PlSqlAnonymousBlock {
        public string Name = "";

        public bool IsExternal = false;

        public bool IsDeclaration = true;

        public List<PlSqlArgument> Arguments = new ();

        protected override (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Name == token.Type) {
                Name = token.Text;
                AddChild(new AstNode(token, "name"));
                return (1, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Special == token.Type) {
                switch(token.Text) {
                    case "(": return (2, StateResult.Continue);
                    case ";": return (-1, StateResult.Return);
                    default:
                        #warning todo error
                        return (-1, StateResult.Return);
                }
            }
            if (TokenType.Keyword == token.Type) {
                return State3(enumerator);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator) {
            Arguments.Add(AddChild(new PlSqlArgument(enumerator)));
            var token = enumerator.Current;
            if (TokenType.Special == token.Type) {
                switch(token.Text) {
                    case ")": return (3, StateResult.Continue);
                    case ";": return (-1, StateResult.Return);
                    case ",": return (2, StateResult.Continue);
                    default:
                        #warning todo error
                        return (-1, StateResult.Return);
                }
            }
            if (TokenType.Keyword == token.Type) {
                return (3, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State3(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Keyword == token.Type) {
                switch(token.GetPlSqlText()) {
                    case "IS": return (4, StateResult.Continue);
                    case "AS": return (5, StateResult.Continue);
                    default:
                       #warning todo error
                        return (-1, StateResult.Return);
                }
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State4(IEnumerator<Token> enumerator) {
            var (state, result) = base.State0(enumerator);
            if (0 == state) state = 4;
            return (state, result);
        }

        protected virtual (int, StateResult) State5(IEnumerator<Token> enumerator) {
            #warning todo external procedure
            return (-1, StateResult.Return);
        }
        
        protected override void InitStates() {
            stateActions.Add(State0);
            stateActions.Add(State1);
            stateActions.Add(State2);
            stateActions.Add(State3);
            stateActions.Add(State4);
            stateActions.Add(State5);
        }

        public PlSqlProcedure(IEnumerator<Token> enumerator) : base(enumerator) {
            Type = "procedure";
        }
    }

    class PlSqlFunction : PlSqlProcedure {
        public IAstNode ReturnType;

        protected override (int, StateResult) State3(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Keyword == token.Type) {
                switch(token.GetPlSqlText()) {
                    case "RETURN": return (6, StateResult.Continue);
                    default:
                       #warning todo error
                        return (-1, StateResult.Return);
                }
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State6(IEnumerator<Token> enumerator) {
            ReturnType = AddChild(new PlSqlType(enumerator));
            var token = enumerator.Current;
            if (TokenType.Special == token.Type) {
                switch(token.Text) {
                    case ")": return (3, StateResult.Continue);
                    case ";": return (-1, StateResult.Return);
                    case ",": return (2, StateResult.Continue);
                    default:
                        #warning todo error
                        return (-1, StateResult.Return);
                }
            }
            if (TokenType.Keyword == token.Type) {
                return (3, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected override void InitStates()
        {
            base.InitStates();
            stateActions.Add(State6);
        }

        public PlSqlFunction(IEnumerator<Token> enumerator) : base(enumerator) {
            Type = "function";
        }
    }

    class PlSqlAnonymousBlock : AstNodeParser {
        // public string Name = "";

        public List<PlSqlTypeDeclaration> Types = new ();

        public List<IAstNode> Cursors = new ();

        public List<PlSqlVariable> Variables = new ();
        
        public List<PlSqlProcedure> Procedures = new ();

        public List<PlSqlFunction> Functions = new ();

        public PlSqlBlock? Block = null;

        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Keyword == token.Type) {
                switch(token.GetPlSqlText()) {
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
                        Block = AddChild(new PlSqlBlock(enumerator));
                        return (-1, StateResult.Return);
                    default:
                        #warning todo error
                        return (-1, StateResult.Return);
                }
            }
            else if (TokenType.Name == token.Type) {
                Variables.Add(AddChild(new PlSqlVariable(enumerator)));
                return (0, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

        protected override void InitStates() {
            stateActions.Add(State0);
        }

        public PlSqlAnonymousBlock(IEnumerator<Token> enumerator) : base(enumerator, "anonymous block") { }
    }

    class PlSqlBlock : AstNodeParser {
        protected TokenType EndType;
        protected string EndText;

        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            switch (token.Type) {
                case TokenType.Keyword:
                    switch (token.GetPlSqlText()) {
                        case "NULL":
                            AddChild(new AstNode(token, "null"));
                            break;
                        case "IF":
                            AddChild(new PlSqlIf(enumerator));
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
                            AddChild(new PlSqlBlock(enumerator));
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

        protected void End(IEnumerator<Token> enumerator) {
            while (enumerator.MoveNext()) {
                var token = enumerator.Current;
                if (TokenType.Commentary == token.Type) {
                    continue;
                }
                if (TokenType.Error == token.Type) {
                    continue;
                }
                if (TokenType.Name == token.Type) {
                    continue;
                }
                if (TokenType.Special == token.Type) {
                    return;
                }
            }
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

        protected override void InitStates()
        {
            stateActions.Add(State0);
        }
        public PlSqlBlock(IEnumerator<Token> enumerator, TokenType endType = TokenType.None, string endText = ""): base(enumerator.Current, "block") { 
            EndType = endType;
            EndText = endText;
            InitStates();
            Parse(enumerator);
        }
    }

    class PlSqlIf : AstNodeParser {
        public List<PlSqlExpression> Conditions = new ();
        public List<PlSqlBlock> Blocks = new ();

        protected (PlSqlExpression? condition, AstNode actions) currentBlock;

        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            Conditions.Add(AddChild(new PlSqlExpression(enumerator, TokenType.Keyword, "THEN")));
            return (1, StateResult.Continue);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            Blocks.Add(AddChild(new PlSqlBlock(enumerator)));
            var token = enumerator.Current;
            return (0, StateResult.Continue);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

        protected override void InitStates()
        {
            stateActions.Add(State0);
            stateActions.Add(State1);
        }

        public PlSqlIf(IEnumerator<Token> enumerator) : base(enumerator.Current, "if") {
            
        }
    }

    class PlSqlLoop : PlSqlBlock {
        protected override (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            switch (token.Type) {
                case TokenType.Keyword:
                    switch (token.GetPlSqlText()) {
                    }
                    break;
            }
            return (-1, StateResult.Return);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

        protected override void InitStates()
        {
            stateActions.Add(State0);
        }

        public PlSqlLoop(IEnumerator<Token> enumerator) : base(enumerator) {
            Type = "loop";
        }
    }

    class PlSqlFor : AstNodeParser {
        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            switch (token.Type) {
                case TokenType.Keyword:
                    switch (token.GetPlSqlText()) {
                    }
                    break;
            }
            return (-1, StateResult.Return);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

        protected override void InitStates()
        {
            stateActions.Add(State0);
        }

        public PlSqlFor(IEnumerator<Token> enumerator) : base(enumerator, "for") { }
    }

    class PlSqlSelect : AstNodeParser {
        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            switch (token.Type) {
                case TokenType.Keyword:
                    switch (token.GetPlSqlText()) {
                    }
                    break;
            }
            return (-1, StateResult.Return);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseNext(enumerator);

        protected override void InitStates()
        {
            stateActions.Add(State0);
        }

        public PlSqlSelect(IEnumerator<Token> enumerator) : base(enumerator, "select") { }
    }

    class PlSqlName: AstNodeParser {
        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Name == token.Type ) {
                AddChild(new AstNode(token, "name"));
                return (1, StateResult.Continue);
            }
            else if (TokenType.Keyword == token.Type) {
                AddChild(new AstNode(token, "name"));
                return (1, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Special == token.Type) {
                switch(token.Text) {
                    case ";":
                    case ",":
                    case "(":
                    case ")":
                        return (-1, StateResult.Return);
                    case ".":
                        AddChild(new AstNode(token, "member"));
                        return (0, StateResult.Continue);
                    case "%":
                        AddChild(new AstNode(token, "attribute"));
                        return (0, StateResult.Continue);
                    case "@":
                        AddChild(new AstNode(token, "remote"));
                        return (0, StateResult.Continue);
                    default:
                        #warning todo error
                        return (-1, StateResult.Return);
                }
            }
            else if (TokenType.Keyword == token.Type) {
                switch(token.GetPlSqlText()) {
                    case "AS":
                    case "END":
                        return (-1, StateResult.Return);
                }
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

        protected override void InitStates() {
            stateActions.Add(State0);
            stateActions.Add(State1);
        }

        public PlSqlName(IEnumerator<Token> enumerator) : base(enumerator, "name") { }
    }

    class PlSqlTypeDeclaration: AstNodeParser {
        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

        protected override void InitStates() {
            stateActions.Add(State0);
        }

        public PlSqlTypeDeclaration(IEnumerator<Token> enumerator) : base(enumerator, "type declaration") { }
    }

    class PlSqlType: AstNodeParser {
        public IAstNode Name;
        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Name == token.Type || TokenType.Keyword == token.Type) {
                Name = AddChild(new PlSqlName(enumerator));
                token = enumerator.Current;
                if (TokenType.Special == token.Type) {
                    if ("(" == token.Text) {
                        return (1, StateResult.Continue);
                    }
                }
                return (-1, StateResult.Return);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Special == token.Type) {
                if (")" == token.Text) {
                    enumerator.MoveNext();
                    return (-1, StateResult.Return);
                }
                if ("," == token.Text) {
                    return (1, StateResult.Continue);
                }
            }
            if (TokenType.Value == token.Type) {
                AddChild(new AstNode(token, "dimension"));
                return (1, StateResult.Continue);
            }
            if (TokenType.Name == token.Type) {
                AddChild(new PlSqlName(enumerator));
                return State1(enumerator);
            }
            if (TokenType.Keyword == token.Type) {
                AddChild(new PlSqlName(enumerator));
                return State1(enumerator);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

        protected override void InitStates() {
            stateActions.Add(State0);
            stateActions.Add(State1);
        }

        public PlSqlType(IEnumerator<Token> enumerator) : base(enumerator, "type") { }
    }
        
    class PlSqlVariable: AstNodeParser {
        public string Name;
        public IAstNode VariableType;
        public IAstNode? Default = null;

        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Name == token.Type) {
                Name = token.Text;
                return (1, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            VariableType = AddChild(new PlSqlType(enumerator));
            var token = enumerator.Current;
            if (TokenType.Special == token.Type) {
                return (-1, StateResult.Return);
            }
            if (TokenType.Operator == token.Type) {
                if (":=" == token.Text) {
                    return (2, StateResult.Continue);
                }
            }
            if (TokenType.Keyword == token.Type) {
                if ("DEFAULT" == token.GetPlSqlText()) {
                    return (2, StateResult.Continue);
                }
            }
            #warning todo error
            return (-1, StateResult.Return);
        }
        
        protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator) {
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

    class PlSqlArgument: AstNodeParser {
        public string Name;
        public bool IsIn = false;
        public bool IsOut = false;
        public IAstNode ArgumentType;
        public IAstNode? Default = null;

        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Name == token.Type) {
                Name = token.Text;
                return (1, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Keyword == token.Type) {
                if ("IN" == token.GetPlSqlText()) {
                    IsIn = true;
                    return (1, StateResult.Continue);
                }
                if ("OUT" == token.GetPlSqlText()) {
                    IsOut = true;
                    return (1, StateResult.Continue);
                }
            }
            return State2(enumerator);
        }
        
        protected virtual (int, StateResult) State2(IEnumerator<Token> enumerator) {
            ArgumentType = AddChild(new PlSqlType(enumerator));
            var token = enumerator.Current;
            if (TokenType.Special == token.Type) {
                return (-1, StateResult.Return);
            }
            if (TokenType.Keyword == token.Type) {
                return State3(enumerator);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }
        
        protected virtual (int, StateResult) State3(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (TokenType.Keyword == token.Type) {
                if ("DEFAULT" == token.GetPlSqlText())
                    return (4, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State4(IEnumerator<Token> enumerator) {
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

    class PlSqlExpression : AstNodeParser {
        protected TokenType EndType;
        protected string EndText;
        
        protected override void Parse(IEnumerator<Token> enumerator) => ParseCurrent(enumerator);

        protected virtual (int, StateResult) State0(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            switch(token.Type) {
                case TokenType.Keyword:
                    goto case TokenType.Name;
                case TokenType.Name:
                    AddChild(new PlSqlName(enumerator));
                    return State1(enumerator);
                case TokenType.Value:
                    AddChild(new AstNode(token, "value"));
                    return (1, StateResult.Continue);
                case TokenType.Special:
                    if ("(" == token.Text) {
                        return (0, StateResult.Continue);
                    }
                    goto case TokenType.Operator;
                case TokenType.Operator:
                    return State1(enumerator);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected virtual (int, StateResult) State1(IEnumerator<Token> enumerator) {
            var token = enumerator.Current;
            if (EndType == token.Type) {
                var text = TokenType.Keyword == token.Type ? token.GetPlSqlText() : token.Text;
                if (EndText == text)
                    return (-1, StateResult.Return);
            }
            switch(token.Type) {
                case TokenType.Keyword:
                    if ("END" == token.GetPlSqlText()) 
                        return (-1, StateResult.Return);
                    goto case TokenType.Operator;
                case TokenType.Special:
                    switch (token.Text) {
                        case "(":
                            AddChild(new PlSqlExpression(enumerator));
                            return (0, StateResult.Continue);
                        case ")":
                            return (-1, StateResult.Return);
                        case ",":
                            AddChild(new AstNode(token, "operator"));
                            return (0, StateResult.Continue);   
                        case ";":
                            return (-1, StateResult.Return);
                    }
                    #warning todo error
                    return (-1, StateResult.Return);
                case TokenType.Operator:
                    AddChild(new AstNode(token, "operator"));
                    return (0, StateResult.Continue);
            }
            #warning todo error
            return (-1, StateResult.Return);
        }

        protected override void InitStates()
        {
            stateActions.Add(State0);
            stateActions.Add(State1);
        }

        public PlSqlExpression(IEnumerator<Token> enumerator, TokenType endType = TokenType.None, string endText = "") : base(enumerator.Current, "expression") { 
            EndType = endType;
            EndText = endText;
            InitStates();
            Parse(enumerator);
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
                    End(enumerator);
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
                case "END":
                    End(enumerator);
                    break;                    
                default:
                    break;
            }
        }
        return res;
    }

    protected void End(IEnumerator<Token> enumerator) {
        while (enumerator.MoveNext()) {
            var token = enumerator.Current;
            if (TokenType.Commentary == token.Type) {
                continue;
            }
            if (TokenType.Error == token.Type) {
                continue;
            }
            if (TokenType.Name == token.Type) {
                continue;
            }
            if (TokenType.Special == token.Type) {
                return;
            }
        }
    }
}