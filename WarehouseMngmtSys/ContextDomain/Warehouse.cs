using System;
using System.Collections.Generic;

namespace Warehouse.Data;

public partial class Warehouse
{
    public Guid Id { get; set; }

    public string Location { get; set; } = null!;

    public virtual ICollection<Item> Items { get; } = new List<Item>();
}
