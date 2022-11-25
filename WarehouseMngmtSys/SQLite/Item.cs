using System;
using System.Collections.Generic;

namespace Warehouse.Data.SQLite;

public partial class Item
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Price { get; set; } = null!;

    public long InStock { get; set; }

    public virtual ICollection<LineItem> LineItems { get; } = new List<LineItem>();

    public virtual ICollection<Warehouse> Warehouses { get; } = new List<Warehouse>();
}
