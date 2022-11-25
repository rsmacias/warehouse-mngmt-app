using System;
using System.Collections.Generic;

namespace Warehouse.Data.SQLite;

public partial class LineItem
{
    public string Id { get; set; } = null!;

    public string ItemId { get; set; } = null!;

    public long Quantity { get; set; }

    public string? OrderId { get; set; }

    public string? ShippingProviderId { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual ShippingProvider? ShippingProvider { get; set; }
}
