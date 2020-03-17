using System;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Product.Api.Web.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<SaleLog> SaleLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(builder =>
            {
                builder.ToTable("products");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
                builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(200)").IsRequired();
                builder.Property(x => x.Price).HasColumnName("price").HasColumnType("decimal(10, 2)").IsRequired();
                builder.Property(x => x.Qty).HasColumnName("qty").HasColumnType("int").IsRequired();
                builder.Property(x => x.Description).HasColumnName("description").HasColumnType("text");
            });


            modelBuilder.Entity<SaleLog>(builder =>
            {
                builder.ToTable("sale_logs");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
                builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.TransactionId).HasColumnName("transaction_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.ProductId).HasColumnName("product_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.Qty).HasColumnName("qty").HasColumnType("int").IsRequired();
                builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar(20)").IsRequired()
                    .HasConversion(x => x.ToString(), x => (TccState) Enum.Parse(typeof(TccState), x));
                builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
                builder.Property(x => x.Expires).HasColumnName("expires").HasColumnType("timestamp").IsRequired();
                builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired();

                builder.HasOne(x => x.Product).WithMany(x => x.SaleLogs).HasForeignKey(x => x.ProductId);
            });
        }
    }
}