using Microsoft.Data.Sqlite;
using System.Data.SqlClient;

string dataSourcesPath = $"{Directory.GetParent(Directory.GetCurrentDirectory()).FullName}\\WarehouseMngmtData";

// sqlite version
string sqliteConnectionString = $"Data Source={dataSourcesPath}\\warehouse.db";

using SqliteConnection connection_sqlite = new SqliteConnection(sqliteConnectionString);

using SqliteCommand command_sqlite = new SqliteCommand("SELECT * FROM [Orders]", connection_sqlite);

connection_sqlite.Open();

using SqliteDataReader reader_sqlite = command_sqlite.ExecuteReader();

while(reader_sqlite.Read()) {
    Console.WriteLine(reader_sqlite["Id"]);
}

// localdb version
string sqlConnectionString = $"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFilename={dataSourcesPath}\\WarehouseManagement.mdf";

using SqlConnection connection = new SqlConnection(sqlConnectionString);

using SqlCommand command = new SqlCommand("SELECT * FROM [Orders]", connection);

connection.Open();

using SqlDataReader reader = command.ExecuteReader();

while(reader.Read()) {
    Console.WriteLine(reader["Id"]);
}

Console.ReadLine();