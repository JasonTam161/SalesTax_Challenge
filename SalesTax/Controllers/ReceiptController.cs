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

        // GET: Receipts
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

        // GET: Receipts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // GET: Receipts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Receipts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemName,Price,Imported")] Receipt receipt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(receipt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(receipt);
        }

        // GET: Receipts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt.FindAsync(id);
            if (receipt == null)
            {
                return NotFound();
            }
            return View(receipt);
        }

        // POST: Receipts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemName,Price,Imported")] Receipt receipt)
        {
            if (id != receipt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receipt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptExists(receipt.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(receipt);
        }

        // GET: Receipts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receipt = await _context.Receipt
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receipt == null)
            {
                return NotFound();
            }

            return View(receipt);
        }

        // POST: Receipts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receipt = await _context.Receipt.FindAsync(id);
            _context.Receipt.Remove(receipt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptExists(int id)
        {
            return _context.Receipt.Any(e => e.Id == id);
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
