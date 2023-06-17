using System.Text;
using Oracle.ManagedDataAccess.Client;

public class OracleSource : ISource {
    string ConnectionString;

    public OracleSource(string connectionString) {
        ConnectionString = connectionString;
    }

    public IEnumerable<string> Data() {
        using var conn = new OracleConnection(ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "select * from user_source where type not in ('JAVA SOURCE') order by type, name, line";
        var sb = new StringBuilder();
        string currentType = "";
        string currentName = "";
        using var reader = cmd.ExecuteReader();
        while(reader.Read()) {
            var type = reader["type"].ToString();
            if (null == type) continue;
            var name = reader["name"].ToString();
            if (null == name) continue;
            if (currentType != type || currentName != name) {
                if (0 != sb.Length) yield return sb.ToString();    
                sb.Clear();
                currentType = type;
                currentName = name;
            }
            var text = reader["text"].ToString();
            if (null == text) continue;
            sb.AppendLine(text);
        }
        if (0 != sb.Length) yield return sb.ToString();
    }
}