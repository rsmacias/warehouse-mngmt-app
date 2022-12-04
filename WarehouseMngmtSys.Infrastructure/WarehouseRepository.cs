namespace warehouseManagementSystem.Infrastructure;

public class WarehouseRepository : GenericRepository<Warehouse> {

    public WarehouseRepository(WarehouseContext context) : base(context) {
        
    }
}