namespace warehouseManagementSystem.Infrastructure;

public class UnitOfWork : IUnitOfWork {
    // Shared db-context between the repositories
    private WarehouseContext _context;
    // Repositories
    private IRepository<Customer> _customerRepository;
    private IRepository<Item> _itemRepository;
    private IRepository<ShippingProvider> _shippingProviderRepository;
    private IRepository<Order> _orderRepository;

    // "Lazy Initialization" for the repositories
    public IRepository<Customer> CustomerRepository {
        get {
            if (_customerRepository is null) {
                _customerRepository = new CustomerRepository(_context);
            }
            return _customerRepository;
        }
    }

    public IRepository<Item> ItemRepository {
        get {
            if(_itemRepository is null) {
                _itemRepository = new ItemRepository(_context);
            }
            return _itemRepository;
        }
    }

    public IRepository<ShippingProvider> ShippingProviderRepository {
        get {
            if (_shippingProviderRepository is null) {
                _shippingProviderRepository = new ShippingProviderRepository(_context);
            }
            return _shippingProviderRepository;
        }
    }

    public IRepository<Order> OrderRepository {
        get {
            if (_orderRepository is null) {
                _orderRepository = new OrderRepository(_context);
            }
            return _orderRepository;
        }
    }

    public UnitOfWork(WarehouseContext context) {
        _context = context;
    }

    public void SaveChanges() => _context.SaveChanges();
    
}