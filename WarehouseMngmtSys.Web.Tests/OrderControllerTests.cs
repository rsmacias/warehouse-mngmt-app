using Moq;
using warehouseManagementSystem.Infrastructure;
using warehouseManagementSystem.Web.Controllers;
using warehouseManagementSystem.Web.Models;

namespace warehouseManagementSystem.Web.Tests;

[TestClass]
public class OrderControllerTests
{
    [TestMethod]
    public void CanCreateOrderWithCorrectModel() {
        // Test Pattern:
        // ARRANGE
        var orderRepository = new Mock<IRepository<Order>>();
        var itemRepository = new Mock<IRepository<Item>>();
        var shippingProviderRepository = new Mock<IRepository<ShippingProvider>>();

        // Return a fake ShippingProvider when All() is called.
        shippingProviderRepository.Setup(
            repoistory => repoistory.All()
        ).Returns(new [] { new ShippingProvider() });

        var orderController = new OrderController(
            orderRepository.Object, 
            shippingProviderRepository.Object, 
            itemRepository.Object
        );

        var createOrderModel = new CreateOrderModel {
            Customer = new () {
                Name = "Robert Macias",
                Address = "Cdla. Buenavista Mz 15 Sl. 5",
                PostalCode = "83465",
                Country = "Ecuador",
                PhoneNumber = "+593 9898 483 01"
            },
            LineItems = new [] {
                new LineItemModel {
                    ItemId = Guid.NewGuid(),
                    Quantity = 100
                }
            }
        };
        
        // ACT
        orderController.Create(createOrderModel);

        // ASSERT
        orderRepository.Verify (
            repository => repository.Add(It.IsAny<Order>()),
            Times.AtMostOnce()
        );
    }
}