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
                    price = price + CalculateItemTax(price, Constants.IMPORT, 0, Constants.ROUND_NUMBER);
                }
                else
                {
                    price = price + CalculateItemTax(price, Constants.IMPORT, Constants.GST, Constants.ROUND_NUMBER);
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
                    price = price + CalculateItemTax(price, 0, Constants.GST, Constants.ROUND_NUMBER);
                }
            }
            return price;
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
            return Math.Round(totalTax*20, MidpointRounding.ToPositiveInfinity) / 20;
        }

        /// <summary>
        /// Calculate tax for item, rounded up to 0.05
        /// </summary>
        /// <param name="price">Price of item</param>
        /// <param name="importTax">Constant of Import Tax. Put 0 if not imported</param>
        /// <param name="gstTax">Constant of gst Tax. Put 0 if tax exempt</param>
        /// <param name="roundTo">Round to decimal position</param>
        /// <returns></returns>
        private decimal CalculateItemTax(decimal price, decimal importTax, decimal gstTax, decimal roundTo)
        {
            return Math.Round((price * (importTax + gstTax)) / 100 * roundTo, MidpointRounding.ToPositiveInfinity) / roundTo;
        }
    }
}
