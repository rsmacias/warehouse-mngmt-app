using System;
using System.Collections.Generic;

namespace Warehouse.Data.SQLite;

public partial class Order
{
    public string Id { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public string ShippingProviderId { get; set; } = null!;

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<LineItem> LineItems { get; } = new List<LineItem>();

    public virtual ShippingProvider ShippingProvider { get; set; } = null!;
}
