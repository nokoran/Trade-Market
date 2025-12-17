using Data.Entities;
using Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) : base(options)
        {
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Receipt> Receipts { get; set; }
        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasOne(p => p.Person)
                .WithOne()
                .HasForeignKey<Customer>(p => p.PersonId);
            
            modelBuilder.Entity<Receipt>()
                .HasMany<ReceiptDetail>(x => x.ReceiptDetails)
                .WithOne(y => y.Receipt)
                .HasForeignKey(x => x.ReceiptId);

            modelBuilder.ApplyConfiguration(new ReceiptDetailConfiguration());
            
            modelBuilder.Entity<Product>()
                .HasOne(x => x.Category)
                .WithMany(y => y.Products)
                .HasForeignKey(x => x.ProductCategoryId);

        }
    }
}
