using System;
using System.Collections.Generic;

namespace Warehouse.Data;

public partial class ShippingProvider
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal FreightCost { get; set; }

    public virtual ICollection<LineItem> LineItems { get; } = new List<LineItem>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
