using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace warehouseManagementSystem.Infrastructure;

public class OrderRepository : GenericRepository<Order> {

    public OrderRepository(WarehouseContext context) : base(context) {
        
    }

    public override IEnumerable<Order> Find(Expression<Func<Order, bool>> predicate) {
        return context.Orders
                        .Include(order => order.LineItems)         // Eager Loading
                        .ThenInclude(lineItem => lineItem.Item)    // Eager Loading
                        .Where(predicate)
                        .ToList();
    }
}