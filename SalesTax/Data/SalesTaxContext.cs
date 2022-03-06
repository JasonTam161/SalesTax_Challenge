#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesTax.Models;

namespace SalesTax.Data
{
    public class SalesTaxContext : DbContext
    {
        public SalesTaxContext (DbContextOptions<SalesTaxContext> options)
            : base(options)
        {
        }

        public DbSet<SalesTax.Models.Items> Items { get; set; }
        public DbSet<ShoppingCart> ShoppingCartItems { get; set; }
        public DbSet<SalesTax.Models.Receipt> Receipt { get; set; }
    }
}
