using Microsoft.EntityFrameworkCore;

namespace warehouseManagementSystem.Web.Data
{
    public class WarehouseContext
        : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<LineItem> LineItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ShippingProvider> ShippingProviders { get; set; }

        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) {
            
        }
        
    }
}
