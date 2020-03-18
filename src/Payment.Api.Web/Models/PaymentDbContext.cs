using System;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Payment.Api.Web.Models
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(builder =>
            {
                builder.ToTable("accounts");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
                builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.Balance).HasColumnName("balance").HasColumnType("decimal(10, 2)").IsRequired();
            });

            modelBuilder.Entity<Bill>(builder =>
            {
                builder.ToTable("bills");

                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).HasColumnName("id").HasColumnType("bigint").IsRequired().ValueGeneratedNever();
                builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("decimal(10, 2)").IsRequired();
                builder.Property(x => x.TransactionId).HasColumnName("transaction_id").HasColumnType("bigint").IsRequired();
                builder.Property(x => x.State).HasColumnName("state").HasColumnType("varchar(20)").IsRequired()
                    .HasConversion(x => x.ToString(), x => (TccState) Enum.Parse(typeof(TccState), x));
                builder.Property(x => x.CreatedAt).HasColumnName("created_at").HasColumnType("timestamp").IsRequired();
                builder.Property(x => x.Expires).HasColumnName("expires").HasColumnType("timestamp").IsRequired();
                builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamp").IsRequired();

                builder.HasOne(x => x.Account).WithMany(x => x.Bills).HasForeignKey(x => x.AccountId);
            });
        }
    }
}