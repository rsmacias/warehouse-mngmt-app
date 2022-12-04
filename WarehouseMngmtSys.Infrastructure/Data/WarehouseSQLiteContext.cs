using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;

namespace warehouseManagementSystem.Infrastructure;

public partial class WarehouseSQLiteContext : WarehouseContext
{

    public WarehouseSQLiteContext(string connectionString)
        : base(connectionString) {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseSqlite(connectionString);
        optionsBuilder.UseLoggerFactory(
            new LoggerFactory(new[] {
                new DebugLoggerProvider()
            })
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasMany(d => d.Warehouses).WithMany(p => p.Items)
                .UsingEntity<Dictionary<string, object>>(
                    "ItemWarehouse",
                    r => r.HasOne<Warehouse>().WithMany().HasForeignKey("WarehousesId"),
                    l => l.HasOne<Item>().WithMany().HasForeignKey("ItemsId"),
                    j =>
                    {
                        j.HasKey("ItemsId", "WarehousesId");
                        j.ToTable("ItemWarehouse");
                        j.HasIndex(new[] { "WarehousesId" }, "IX_ItemWarehouse_WarehousesId");
                    });
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.ToTable("LineItem");

            entity.HasIndex(e => e.ItemId, "IX_LineItem_ItemId");

            entity.HasIndex(e => e.OrderId, "IX_LineItem_OrderId");

            entity.HasIndex(e => e.ShippingProviderId, "IX_LineItem_ShippingProviderId");

            entity.HasOne(d => d.Item).WithMany(p => p.LineItems).HasForeignKey(d => d.ItemId);

            entity.HasOne(d => d.Order).WithMany(p => p.LineItems).HasForeignKey(d => d.OrderId);

            entity.HasOne(d => d.ShippingProvider).WithMany(p => p.LineItems).HasForeignKey(d => d.ShippingProviderId);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

            entity.HasIndex(e => e.ShippingProviderId, "IX_Orders_ShippingProviderId");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders).HasForeignKey(d => d.CustomerId);

            entity.HasOne(d => d.ShippingProvider).WithMany(p => p.Orders).HasForeignKey(d => d.ShippingProviderId);
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.ToTable("Warehouse");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
