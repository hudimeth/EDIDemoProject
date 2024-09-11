using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EDIConverterWeb.Data
{
    public class EDIDbContext : DbContext
    {
        private string _connectionString;
        public EDIDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<POAcknowledgement>()
                .HasKey(poa => poa.ReferenceNumber);
            modelBuilder.Entity<POAcknowledgement>(b =>
            {
                b.ToTable("PurchaseOrderAcknowledgements");
                b.Property(x => x.ReferenceNumber).ValueGeneratedOnAdd()
                .UseIdentityColumn(1000000001, 100001);
            });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<POAcknowledgement> PurchaseOrderAcknowledgements { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Item> ItemsOrdered { get; set; }
    }
}
