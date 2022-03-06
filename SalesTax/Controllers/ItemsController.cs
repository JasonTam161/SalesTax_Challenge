#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesTax.Data;
using SalesTax.Global;
using SalesTax.Helpers;
using SalesTax.Models;

namespace SalesTax.Controllers
{
    public class ItemsController : Controller
    {
        private readonly SalesTaxContext _context;

        public ItemsController(SalesTaxContext context)
        {
            _context = context;
        }

        // GET: Items2
        public async Task<IActionResult> Index()
        {
            //Set up TempData to display items in cart
            if (HttpContext.Session.Get(Constants.CART) == null)
            {
                TempData["ItemsInCart"] = "0 Items in cart.";
            }
            else
            {
                List<Items> cartItems = HttpContext.Session.Get<List<Items>>(Constants.CART);
                TempData["ItemsInCart"] = string.Format("{0} items in cart", cartItems.Count);
            }
            return View(await _context.Items.ToListAsync());
        }

        private bool ItemsExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
