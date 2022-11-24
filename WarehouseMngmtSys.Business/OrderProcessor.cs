using warehouseManagementSystem.Domain;

namespace warehouseManagementSystem.Business;

public class OrderProcessor {

    private void Initialize (Order order) {
        Console.WriteLine($"Initializing Order with Order number: {order.Id}");
    }

    public void Process (Order order) {
        Initialize(order);
        Console.WriteLine($"Finalizing Order Processing for Order number: {order.Id}");
    }

}
