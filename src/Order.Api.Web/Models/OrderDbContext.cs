using System;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Order.Api.Web.Models
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("orders");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
                builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.TransactionId).HasColumnName("transaction_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar(20)").IsRequired()
                    .HasConversion(x => x.ToString(), x => (TccState) Enum.Parse(typeof(TccState), x));
                builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
                builder.Property(x => x.Expires).HasColumnName("expires").HasColumnType("timestamp").IsRequired();
                builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired();
            });

            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.ToTable("order_items");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
                builder.Property(x => x.OrderId).HasColumnName("order_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.ProductId).HasColumnName("product_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.Price).HasColumnName("price").HasColumnType("decimal(10, 2)").IsRequired();
                builder.Property(x => x.Qty).HasColumnName("qty").HasColumnType("int").IsRequired();

                builder.HasOne(x => x.Order).WithMany(x => x.Items).HasForeignKey(x => x.OrderId);
            });
        }
    }
}