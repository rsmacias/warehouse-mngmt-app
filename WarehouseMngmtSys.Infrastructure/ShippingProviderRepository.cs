namespace warehouseManagementSystem.Infrastructure;

public class ShippingProviderRepository : GenericRepository<ShippingProvider> {

    public ShippingProviderRepository(WarehouseContext context) : base(context) {
        
    }

}