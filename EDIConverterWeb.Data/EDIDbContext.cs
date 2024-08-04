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
            modelBuilder.Entity<PurchaseOrderAcknowledgement>()
                .HasKey(poa => poa.InterchangeId);

            modelBuilder.Entity<PurchaseOrderAcknowledgement>(b =>
            {
                b.ToTable("PurchaseOrderAcknowledgements");
                b.Property(x => x.InterchangeId).ValueGeneratedOnAdd()
                .UseIdentityColumn(100010001, 10001);
            });

            ////these 2 don't work cuz they are considered identity columns and you are only allowed 1 identity column per table.
            ////needa figure out how to do this without considering it an identity
            //modelBuilder.Entity<PurchaseOrderAcknowledgement>()
            //    .Property(poa => poa.InterchangeNumber).ValueGeneratedOnAdd()
            //    .UseIdentityColumn(000000006, 1);

            //modelBuilder.Entity<PurchaseOrderAcknowledgement>()
            //    .Property(poa => poa.GroupNumber).ValueGeneratedOnAdd()
            //    .UseIdentityColumn(00010, 1);
        }

        public DbSet<PurchaseOrderAcknowledgement> PurchaseOrderAcknowledgements { get; set; }
        public DbSet<Item> ItemsOrdered { get; set; }
    }
}
