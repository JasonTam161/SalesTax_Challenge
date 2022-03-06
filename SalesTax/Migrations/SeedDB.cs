using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesTax.Data;
using System;
using System.Linq;

namespace SalesTax.Models
{
    //Required, as Database will be refreshed each time it's loaded/executed on start up
    public class SeedDB
    {
        public static void Init(IServiceProvider serviceProvider)
        {
            using (var context = new SalesTaxContext(serviceProvider.GetRequiredService<DbContextOptions<SalesTaxContext>>()))
            {
                //Check if DB has items
                if (context.Items.Any())
                {
                    return; //Database already has items
                }

                //Add items to DB on startup and if no items exists
                context.Items.AddRange(
                        new Items
                        {
                            ItemName = "Book",
                            Price = 12.49M,
                            Imported = false,
                            GSTExempt = true,
                            QuantityId = 1
                        },

                        new Items
                        {
                            ItemName = "Music CD",
                            Price = 14.99M,
                            Imported = false,
                            GSTExempt = false,
                            QuantityId = 2
                        },

                        new Items
                        {
                            ItemName = "Chocolate Bar",
                            Price = 0.85M,
                            Imported = false,
                            GSTExempt = true,
                            QuantityId = 3
                        },

                        new Items
                        {
                            ItemName = "Imported Box of Chocolates",
                            Price = 10.00M,
                            Imported = true,
                            GSTExempt = true,
                            QuantityId = 4
                        },

                        new Items
                        {
                            ItemName = "Imported Bottle of Perfume",
                            Price = 47.50M,
                            Imported = true,
                            GSTExempt = false,
                            QuantityId = 5
                        },

                        new Items
                        {
                            ItemName = "Imported Bottle of Perfume #2",
                            Price = 27.99M,
                            Imported = true,
                            GSTExempt = false,
                            QuantityId = 6
                        },

                        new Items
                        {
                            ItemName = "Perfume",
                            Price = 18.99M,
                            Imported = false,
                            GSTExempt = false,
                            QuantityId = 7
                        },

                        new Items
                        {
                            ItemName = "Headache Pills",
                            Price = 9.75M,
                            Imported = false,
                            GSTExempt = true,
                            QuantityId = 8
                        },

                        new Items
                        {
                            ItemName = "Imported Box of Chocolates #2",
                            Price = 11.25M,
                            Imported = true,
                            GSTExempt = true,
                            QuantityId = 9
                        }
                    ) ;
                context.SaveChanges();

            }
        }
    }
}
