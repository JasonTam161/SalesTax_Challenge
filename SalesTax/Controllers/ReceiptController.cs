#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesTax.Data;
using SalesTax.Models;
using SalesTax.Global;
using Microsoft.AspNetCore.Http;
using SalesTax.Helpers;

namespace SalesTax.Controllers
{
    public class ReceiptController : Controller
    {
        private readonly SalesTaxContext _context;

        public ReceiptController(SalesTaxContext context)
        {
            _context = context;
        }

        //Calculate taxes on load
        public ActionResult Index()
        {
            List<Receipt> shoppingCart = new List<Receipt>();
            shoppingCart = HttpContext.Session.Get<List<Receipt>>(Constants.CART);
            //Clear session after loading, as user would've checked out
            HttpContext.Session.Clear();
            decimal totalTax = 0M;
            decimal totalAmount = 0M;
            foreach(var item in shoppingCart)
            {
                //Get new item price, where tax is added
                totalTax = CalculateTotalTax(totalTax, item.Price, item.Imported, item.GSTExempt);
                item.Price = AddTax(item.Price, item.Imported, item.GSTExempt);
                totalAmount = totalAmount + item.Price;
            }
            shoppingCart.ForEach(x => x.TotalTax = totalTax);
            shoppingCart.ForEach(x => x.TotalPrice = totalAmount);
            return View(shoppingCart.ToList());
        }

        private decimal AddTax(decimal price, bool importedItem, bool GSTExempt)
        {
            //Add Tax
            if (importedItem)
            {
                if (GSTExempt)
                {
                    price = price + (price * Constants.IMPORT)/100;
                }
                else
                {
                    price = price + (price * (Constants.GST + Constants.IMPORT))/100;
                }

            }
            else
            {
                if (GSTExempt)
                {
                    //No Tax if GST Exempt AND not imported
                    return price;
                }
                else
                {
                    price = price + (price * Constants.GST)/100;
                }
            }

            //Return new price
            return Math.Round(price*20, MidpointRounding.AwayFromZero)/20;
        }

        private decimal CalculateTotalTax (decimal totalTax, decimal price, bool importedItem, bool GSTExempt)
        {
            if (importedItem)
            {
                if (GSTExempt)
                {
                    totalTax = totalTax + (price * Constants.IMPORT) / 100;
                }
                else
                {
                    totalTax = totalTax + (price * (Constants.GST + Constants.IMPORT)) / 100;
                }

            }
            else
            {
                if (GSTExempt)
                {
                    //No Tax if GST Exempt AND not imported
                    return totalTax;
                }
                totalTax = totalTax + (price * Constants.GST) / 100;
            }

            return Math.Round(totalTax*20, MidpointRounding.AwayFromZero) / 20;
        }
    }
}
