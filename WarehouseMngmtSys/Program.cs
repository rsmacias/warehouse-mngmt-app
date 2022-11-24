using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

const string DATASOURCE_KEY_TO_REPLACE = "{dataSourcesPath}";
bool isWindowsPlatform = config.GetValue<bool>("Settings:WindowsPlatform");
string dataSourcesDirectory = config.GetValue<string>("Settings:DataSourcesDirectory") ?? "\\WarehouseMngmtData";
Console.WriteLine($"isWindowsPlatform: {isWindowsPlatform}");

string dataSourcesPath = $"{Directory.GetParent(Directory.GetCurrentDirectory()).FullName}\\{dataSourcesDirectory}";
string connectionString = string.Empty;

if (!isWindowsPlatform) {
    // sqlite version for Cross Platform
    connectionString = config.GetConnectionString("SqliteVersionDb") ?? throw new ArgumentNullException();
    connectionString = connectionString.Replace(DATASOURCE_KEY_TO_REPLACE, dataSourcesPath);

    using SqliteConnection connection_sqlite = new SqliteConnection(connectionString);

    using SqliteCommand command_sqlite = new SqliteCommand("SELECT * FROM [Orders]", connection_sqlite);

    connection_sqlite.Open();

    using SqliteDataReader reader_sqlite = command_sqlite.ExecuteReader();

    while(reader_sqlite.Read()) {
        Console.WriteLine(reader_sqlite["Id"]);
    }
} else {
    // localdb version for Windows Platform
    connectionString = config.GetConnectionString("SqlVersionDb") ?? throw new ArgumentNullException();
    connectionString = connectionString.Replace(DATASOURCE_KEY_TO_REPLACE, dataSourcesPath);

    //string sqlConnectionString = $"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFilename={dataSourcesPath}\\WarehouseManagement.mdf";

    using SqlConnection connection = new SqlConnection(connectionString);

    using SqlCommand command = new SqlCommand("SELECT * FROM [Orders]", connection);

    connection.Open();

    using SqlDataReader reader = command.ExecuteReader();

    while(reader.Read()) {
        Console.WriteLine(reader["Id"]);
    }
}

Console.ReadLine();