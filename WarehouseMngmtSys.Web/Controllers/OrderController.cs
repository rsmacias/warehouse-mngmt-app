using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using warehouseManagementSystem.Infrastructure;
using warehouseManagementSystem.Web.Models;

namespace WarehouseManagementSystem.Web.Controllers;

public class OrderController : Controller {

    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<ShippingProvider> _shippingProviderRepository;
    private readonly IRepository<Item> _itemRepository;

    public OrderController(IRepository<Order> orderRepository, 
                           IRepository<ShippingProvider> shippingProviderRepository, 
                           IRepository<Item> itemRepository) {
        _orderRepository = orderRepository;
        _shippingProviderRepository = shippingProviderRepository;
        _itemRepository = itemRepository;
    }

    public IActionResult Index() {
        var orders = 
            _orderRepository.Find(
                    order => 
                    order.CreatedAt > DateTime.UtcNow.AddDays(-1)
            );

        return View(orders);
    }

    public IActionResult Create() {
        var items = _itemRepository.All();  

        return View(items);
    }

    [HttpPost]
    public IActionResult Create(CreateOrderModel model) {
        #region Validate input
        if (!model.LineItems.Any()) return BadRequest("Please submit line items");

        if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");
        #endregion

        var customer = new Customer
        {
            Name = model.Customer.Name,
            Address = model.Customer.Address,
            PostalCode = model.Customer.PostalCode,
            Country = model.Customer.Country,
            PhoneNumber = model.Customer.PhoneNumber
        };

        var order = new Order
        {
            LineItems = model.LineItems
                .Select(line => new LineItem { 
                    Id = Guid.NewGuid(), 
                    ItemId = line.ItemId, 
                    Quantity = line.Quantity
                })
                .ToList(),

            Customer = customer,
            ShippingProviderId = _shippingProviderRepository.All().First().Id, 
            CreatedAt = DateTimeOffset.UtcNow
        };

        _orderRepository.Add(order);
        _orderRepository.SaveChanges();

        return Ok("Order Created");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
