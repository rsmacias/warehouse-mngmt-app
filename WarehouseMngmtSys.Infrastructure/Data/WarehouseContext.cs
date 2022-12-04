using Microsoft.EntityFrameworkCore;

namespace warehouseManagementSystem.Infrastructure
{
    public class WarehouseContext
        : DbContext
    {
        protected readonly string connectionString;

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShippingProvider> ShippingProviders { get; set; }

        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) {
            
        }

        public WarehouseContext(string connectionString) {
            this.connectionString = connectionString;
        }
        
    }
}
