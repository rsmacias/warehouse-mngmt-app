using warehouseManagementSystem.Web.Data;
using Microsoft.EntityFrameworkCore;

const string DATASOURCE_KEY_TO_REPLACE = "{dataSourcesPath}";

var GetConnectionString = string (bool isWindowsPlatform, IConfiguration config) => {
    string connectionString = string.Empty;
    string dataSourcesDirectory = config.GetValue<string>("Settings:DataSourcesDirectory") ?? "\\WarehouseMngmtData";

    string dataSourcesPath = $"{Directory.GetParent(Directory.GetCurrentDirectory()).FullName}\\{dataSourcesDirectory}";

    connectionString = isWindowsPlatform ? config.GetConnectionString("SqlVersionDb") ?? throw new ArgumentNullException() 
                                        : config.GetConnectionString("SqliteVersionDb") ?? throw new ArgumentNullException();

    if (connectionString.Contains(DATASOURCE_KEY_TO_REPLACE)) {
        connectionString = connectionString.Replace(DATASOURCE_KEY_TO_REPLACE, dataSourcesPath);
    }

    return connectionString;
};

var builder = WebApplication.CreateBuilder(args);

bool isWindowsPlatform = builder.Configuration.GetValue<bool>("Settings:WindowsPlatform");
Console.WriteLine($"isWindowsPlatform: {isWindowsPlatform}");

string connectionString = GetConnectionString(isWindowsPlatform, builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Data Context as a service 
if(isWindowsPlatform){
    builder.Services.AddDbContext<WarehouseContext>(options => options.UseSqlServer(connectionString));
} else {
    builder.Services.AddDbContext<WarehouseContext>(options => options.UseSqlite(connectionString));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
