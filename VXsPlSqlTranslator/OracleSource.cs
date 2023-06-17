using Oracle.ManagedDataAccess.Client;

public class OracleSource : ISource {
    string ConnectionString;

    public OracleSource(string connectionString) {
        ConnectionString = connectionString;
    }

    public IEnumerable<char> Data() {
        using var conn = new OracleConnection(ConnectionString);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "select * from user_source where type not in ('JAVA SOURCE') order by type, name, line";
        using var reader = cmd.ExecuteReader();
        while(reader.Read()) {
            var text = reader["text"].ToString();
            if (string.IsNullOrEmpty(text)) continue;
            foreach(var c in text) yield return c;
        }
    }
}