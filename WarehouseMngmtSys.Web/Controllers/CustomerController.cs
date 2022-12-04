using Microsoft.AspNetCore.Mvc;
using warehouseManagementSystem.Web.Data;

namespace warehouseManagementSystem.Web.Controllers;

public class CustomerController : Controller {
    private readonly WarehouseContext context = null;

    public CustomerController(WarehouseContext context) {
        this.context = context;
    }

    public IActionResult Index(Guid? id) {
        if (id == null) {
            var customers = context.Customers.ToList();

            return View(customers);
        } else {
            var customer = context.Customers.Find(id.Value);

            return View(new[] { customer });
        }
    }
}
