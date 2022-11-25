using System.Data.Common;
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

var showAll = () => {
    foreach (var order in context.Orders
                            .Include(order => order.Customer)
                            .Include(order => order.ShippingProvider)
                            .Include(order => order.LineItems)
                            .ThenInclude(lineItem => lineItem.Item)) {
        Console.WriteLine("------------------------------------------------");
        Console.WriteLine($"Order Id: {order.Id}");
        Console.WriteLine($"Customer: {order.Customer.Name}");
        Console.WriteLine($"Shipping Provider: {order.ShippingProvider.Name}");

        foreach (var lineItem in order.LineItems) {
            Console.WriteLine($"\tItem: {lineItem.Item.Name}");
            Console.WriteLine($"\tPrice: {lineItem.Item.Price}");    
        }
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadLine();
};

var newCustomer = () => {
    Console.WriteLine("Enter Customer data...");

    Console.Write($"Name: ");
    var name = Console.ReadLine();
    Console.Write($"Address: ");
    var address = Console.ReadLine();
    Console.Write($"Postal Code: ");
    var zipCode = Console.ReadLine();
    Console.Write($"Country: ");
    var country = Console.ReadLine();
    Console.Write($"Phone Number: ");
    var phoneNumber = Console.ReadLine();

    var newCustomer = new Customer {
        Name = name,
        Address = address,
        PostalCode = zipCode,
        Country = country,
        PhoneNumber = phoneNumber
    };

    context.Customers.Add(newCustomer);
    context.SaveChanges();

    Console.WriteLine("New customer added.\r\nPress any key to continue...");
    Console.ReadLine();
};

var showMenu = () => {
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("0.- Exit");
    Console.WriteLine("1.- Select All");
    Console.WriteLine("2.- New Customer");
};

var gotToOption = (string option) => {
    switch(option) {
        case "1" : showAll(); break;
        case "2" : newCustomer(); break;
        default: break;
    }
};

string option = string.Empty;
do {
    option = string.Empty;
    Console.Clear();
    showMenu();
    Console.Write("Select an option: ");
    option = Console.ReadLine();
    gotToOption(option);
} while(option != "0");

Console.WriteLine("Bye!");
