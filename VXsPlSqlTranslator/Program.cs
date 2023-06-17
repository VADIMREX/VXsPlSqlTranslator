using Newtonsoft.Json;

using VXs.PlSqlTranslator;
using VXs.Utils;

var settings = new Settings {
    inPath = "",
    outPath = ""
};

Dictionary<string, ManInfo> man = new ();

man.Add("-h", new ("this text", i => {
    foreach(var manLine in man) {
        Console.WriteLine($"{manLine.Key}\t{manLine.Value.Description}");
    }
    return args.Length;
}));
man.Add("-input", new ("interactive mode", i => {
    Console.WriteLine("please enter arguments, empty line for continue");
    var lst = new List<string>();
    int j = 0;
    while (j < 2)
    {
        var s = Console.ReadLine();
        if ("" == s)
        {
            j++;
            continue;
        }
        j = 0;
        lst.Add(s);
    }
    args = lst.ToArray();
    return -1;
}));
man.Add("-m", new ("mode:\n\t\tdb - parse db", i => {
    if (i + 1 == args.Length) return i;
    settings.mode = args[++i];
    return i;
}));
man.Add("-i", new ("<file path> set input file", i => {
    if (i + 1 == args.Length) return i;
    settings.inPath = args[++i];
    return i;
}));
man.Add("-o", new ("<file path> set output file ", i => {
    if (i + 1 == args.Length) return i;
    settings.outPath = args[++i];
    return i;
}));
man.Add("-c", new ("<file path> config file ", i => {
    if (i + 1 == args.Length) return i;
    using var sr = new StreamReader(args[++i]);
    settings = JsonConvert.DeserializeObject<Settings>(sr.ReadToEnd());
    return args.Length;
}));

for(var i = 0; i < args.Length; i++) {
    if (!man.ContainsKey(args[i])) {
        Console.WriteLine($"unknown argument: {args[i]}");
        return;
    }
    i = man[args[i]].Action(i);
}

ISource source;
switch (settings.mode) {
    case "db":
        source = new OracleSource(settings.connectionString);
        break;
    default:
        return;
}

var data = source.Data();

var lexer = new VXs.Lexer.PlSqlLexer();
foreach(var str in data) {
    foreach(var token in lexer.Parse(str)) {
        Console.WriteLine(token);
    }
}
