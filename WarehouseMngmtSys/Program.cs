using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Warehouse.Data;
using Warehouse.Data.SQLite;
using Warehouse.Data.SqlServer;

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

using WarehouseContext context = isWindowsPlatform ?  new WarehouseSqlServerContext(connectionString) : new WarehouseSQLiteContext(connectionString);

var showAllOrders = () => {
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
};

var newCustomer = () => {
    Console.WriteLine("==================CREATE CUSTOMER==================");
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

    Console.WriteLine($"Customer created!");
};

var wantToUpdateValueField = bool (string field, string currentValue) => {
    Console.WriteLine($"Current value for {field}: {currentValue}");
    Console.Write($"Do you want to update it? Y/N: ");
    return Console.ReadLine().ToUpper().Equals("Y");
};

var getCustomerField = string (string field, string currentValue) => {
    Console.WriteLine();
    if (wantToUpdateValueField(field, currentValue)){
        Console.Write($"Get new value: ");
        return Console.ReadLine();
    }
    return currentValue;
};

var updateCustomerByName = () => {
    Console.WriteLine("==================UPDATE CUSTOMER==================");
    Console.Write("Enter the name of the customer you want to update: ");
    var filterByName = Console.ReadLine();

    var customerToUpdate = context.Customers.FirstOrDefault(customer => customer.Name == filterByName);
    if (customerToUpdate != null) {
        customerToUpdate.Name = getCustomerField("Name", customerToUpdate.Name);
        customerToUpdate.Address = getCustomerField("Address", customerToUpdate.Address);
        customerToUpdate.PostalCode = getCustomerField("Zip Code", customerToUpdate.PostalCode);
        customerToUpdate.Country = getCustomerField("Country", customerToUpdate.Country);
        customerToUpdate.PhoneNumber = getCustomerField("Phone Number", customerToUpdate.PhoneNumber);

        context.Customers.Update(customerToUpdate);
        context.SaveChanges();

        Console.WriteLine($"Customer updated!");
    } else {
        Console.WriteLine($"Customer not found!");
    }
};

var removeCustomerWithOrders = () => {
    Console.WriteLine("==================DELETE CUSTOMER==================");
    var toDelete = context.Customers.FirstOrDefault(customer => customer.Orders.Any());
    if(toDelete != null) {
        Console.WriteLine($"Customer {toDelete.Name} will be deleted.");
        context.Customers.Remove(toDelete);
        context.SaveChanges();
        Console.WriteLine($"Customer {toDelete.Name} deleted!");
    } else {
        Console.WriteLine("Customer not found!");
    }
};

var showMenu = () => {
    Console.WriteLine("------------------------------------------------");
    Console.WriteLine("0.- Exit");
    Console.WriteLine("1.- Select all Orders");
    Console.WriteLine("2.- New Customer");
    Console.WriteLine("3.- Update Customer");
    Console.WriteLine("4.- Delete Customer with Orders");
};

var gotToOption = (string option) => {
    Console.WriteLine();
    switch(option) {
        case "1" : showAllOrders(); break;
        case "2" : newCustomer(); break;
        case "3" : updateCustomerByName(); break;
        case "4" : removeCustomerWithOrders(); break;
        default: break;
    }
    Console.WriteLine("Press any key to continue...");
    Console.ReadLine();
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

