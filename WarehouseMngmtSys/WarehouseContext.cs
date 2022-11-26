using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Data;

public class WarehouseContext : DbContext {

    protected readonly string connectionString = null;

    public WarehouseContext(string connectionString) {
        this.connectionString = connectionString;
    }

    public WarehouseContext(DbContextOptions<WarehouseContext> options)
        : base(options) {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<ShippingProvider> ShippingProviders { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

}