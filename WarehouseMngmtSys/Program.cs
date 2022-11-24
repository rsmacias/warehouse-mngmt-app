﻿using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using warehouseManagementSystem;


IConfiguration config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

const string DATASOURCE_KEY_TO_REPLACE = "{dataSourcesPath}";
bool isWindowsPlatform = config.GetValue<bool>("Settings:WindowsPlatform");
string dataSourcesDirectory = config.GetValue<string>("Settings:DataSourcesDirectory") ?? "\\WarehouseMngmtData";
Console.WriteLine($"isWindowsPlatform: {isWindowsPlatform}");

string dataSourcesPath = $"{Directory.GetParent(Directory.GetCurrentDirectory()).FullName}\\{dataSourcesDirectory}";
string connectionString = string.Empty;

connectionString = isWindowsPlatform ? config.GetConnectionString("SqlVersionDb") ?? throw new ArgumentNullException() 
                                     : config.GetConnectionString("SqliteVersionDb") ?? throw new ArgumentNullException();

if (connectionString.Contains(DATASOURCE_KEY_TO_REPLACE)) {
    connectionString = connectionString.Replace(DATASOURCE_KEY_TO_REPLACE, dataSourcesPath);
}

var optionsBuilder = new DbContextOptionsBuilder<WarehouseContext>();

if (isWindowsPlatform) {
    optionsBuilder.UseSqlServer(connectionString);
} else {
    optionsBuilder.UseSqlite(connectionString);
}

using var context = new WarehouseContext(optionsBuilder.Options);

foreach (var order in context.Orders) {
    Console.WriteLine($"Order Id: {order.Id}");
}

Console.ReadLine();