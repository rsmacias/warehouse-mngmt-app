namespace warehouseManagementSystem.Infrastructure;

public interface IUnitOfWork {

    IRepository<Customer> CustomerRepository { get; }
    IRepository<Order> OrderRepository { get; }
    IRepository<Item> ItemRepository { get; }
    IRepository<ShippingProvider> ShippingProviderRepository { get; }

    void SaveChanges();

}