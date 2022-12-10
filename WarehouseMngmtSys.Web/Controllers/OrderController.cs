using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using warehouseManagementSystem.Infrastructure;
using warehouseManagementSystem.Web.Models;

namespace warehouseManagementSystem.Web.Controllers;

public class OrderController : Controller {

    private readonly IUnitOfWork _unitOfWork;

    public OrderController(IUnitOfWork unitOfWork) {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index() {
        var orders = 
            _unitOfWork.OrderRepository.Find(
                    order => 
                    order.CreatedAt > DateTime.UtcNow.AddDays(-1)
            );

        return View(orders);
    }

    public IActionResult Create() {
        var items = _unitOfWork.ItemRepository.All();  

        return View(items);
    }

    [HttpPost]
    public IActionResult Create(CreateOrderModel model) {
        #region Validate input
        if (!model.LineItems.Any()) return BadRequest("Please submit line items");

        if (string.IsNullOrWhiteSpace(model.Customer.Name)) return BadRequest("Customer needs a name");
        #endregion

        var customer = _unitOfWork
                        .CustomerRepository
                        .Find(customer => customer.Name == model.Customer.Name)
                        .FirstOrDefault();

        if (customer is null) {
            customer = new Customer {
                Id = Guid.NewGuid(),
                Name = model.Customer.Name,
                Address = model.Customer.Address,
                PostalCode = model.Customer.PostalCode,
                Country = model.Customer.Country,
                PhoneNumber = model.Customer.PhoneNumber
            };
        } else {
            customer.Address = model.Customer.Address;
            customer.PostalCode = model.Customer.PostalCode;
            customer.Country = model.Customer.Country;
            customer.PhoneNumber = model.Customer.PhoneNumber;

            _unitOfWork.CustomerRepository.Update(customer);
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            LineItems = model.LineItems
                .Select(line => new LineItem { 
                    Id = Guid.NewGuid(), 
                    ItemId = line.ItemId, 
                    Quantity = line.Quantity
                })
                .ToList(),

            Customer = customer,
            ShippingProviderId = _unitOfWork.ShippingProviderRepository.All().First().Id, 
            CreatedAt = DateTimeOffset.UtcNow
        };

        _unitOfWork.OrderRepository.Add(order);
        
        _unitOfWork.OrderRepository.SaveChanges();

        return Ok("Order Created");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
