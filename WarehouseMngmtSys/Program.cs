using Microsoft.Data.Sqlite;
using System.Data.SqlClient;


// sqlite version
using SqliteConnection connection_sqlite = new SqliteConnection(@"Data Source=C:\prjs\prs\GitHub\rsmacias\warehouse-mngmt-app\WarehouseMngmtData\warehouse.db");

using SqliteCommand command_sqlite = new SqliteCommand("SELECT * FROM [Orders]", connection_sqlite);

connection_sqlite.Open();

using SqliteDataReader reader_sqlite = command_sqlite.ExecuteReader();

while(reader_sqlite.Read()) {
    Console.WriteLine(reader_sqlite["Id"]);
}

// localdb version
using SqlConnection connection = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFilename=C:\prjs\prs\GitHub\rsmacias\warehouse-mngmt-app\WarehouseMngmtData\WarehouseManagement.mdf");

using SqlCommand command = new SqlCommand("SELECT * FROM [Orders]", connection);

connection.Open();

using SqlDataReader reader = command.ExecuteReader();

while(reader.Read()) {
    Console.WriteLine(reader["Id"]);
}

Console.ReadLine();