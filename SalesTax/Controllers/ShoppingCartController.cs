using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesTax.Data;
using SalesTax.Models;
using Microsoft.AspNetCore.Http;
using SalesTax.Helpers;
using System.Text;
using SalesTax.Global;

namespace SalesTax.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            List<ShoppingCart> sessionCart = HttpContext.Session.Get<List<ShoppingCart>>(Constants.CART);
            return View(sessionCart);
        }

        public ActionResult Add(Items items)
        {
            if(HttpContext.Session.Get(Constants.CART) == null)
            {
                List<Items> cartItems = new List<Items>();
                cartItems.Add(items);
                TempData["ItemsAdded"] = string.Format("{0} has been added to the cart", items.ItemName);
                TempData["ItemsInCart"] = string.Format("{0} items in cart", cartItems.Count);
                HttpContext.Session.Set(Constants.CART, cartItems);
            }
            else
            {
                //Convert Session Cart back to list to add items
                //Then store it back into session
                List<Items> cartItems = HttpContext.Session.Get<List<Items>>(Constants.CART);
                cartItems.Add(items);
                TempData["ItemsAdded"] = string.Format("{0} has been added to the cart", items.ItemName);
                TempData["ItemsInCart"] = string.Format("{0} items in cart", cartItems.Count);
                HttpContext.Session.Set(Constants.CART, cartItems);
            }
            return RedirectToAction("Index", "Items");
        }

        //Delete item
        public ActionResult Delete(int id)
        {
            //Delete from session then rebind it
            List<ShoppingCart> sessionCart = HttpContext.Session.Get<List<ShoppingCart>>(Constants.CART);
            var deletedItem = sessionCart.Find(x => x.Id == id);
            sessionCart.Remove(deletedItem);
            HttpContext.Session.Set(Constants.CART, sessionCart);
            return RedirectToAction("Index", "ShoppingCart", false);
        }

    }
}
