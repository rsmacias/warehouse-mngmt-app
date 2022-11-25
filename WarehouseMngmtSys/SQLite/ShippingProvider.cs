using System;
using System.Collections.Generic;

namespace Warehouse.Data.SQLite;

public partial class ShippingProvider
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string FreightCost { get; set; } = null!;

    public virtual ICollection<LineItem> LineItems { get; } = new List<LineItem>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
