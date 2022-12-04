using Microsoft.EntityFrameworkCore;

namespace warehouseManagementSystem.Infrastructure
{
    public class WarehouseContext
        : DbContext
    {

        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options) {
            
        }
        
    }
}
