using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePizzeria.Models;
using OnlinePizzeria.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlinePizzeria.Controllers
{
    public class CartController : Controller
    {

        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Cart
        public async Task<ActionResult> Index()
        {
            List<CartItem> cartItems = new List<CartItem>();

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                var carts = await _context.Carts.ToListAsync();
                var newId = HttpContext.Session.GetInt32("Cart");
                newId = carts.Count + 1;

                cartItems = new List<CartItem>();
            }

            else
            {
                Cart cart = new Cart();
                var cartId = HttpContext.Session.GetInt32("Cart");
                cart = await _context.Carts.Include(x => x.Items).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartId == cartId);
                cartItems = cart.Items;
            }
            return View(cartItems);
        }

        // GET: Cart/Details/5
        public async Task<ActionResult> AddToCart(int id)
        {
            Dish dish = _context.Dishes.FirstOrDefault(x => x.Id == id);

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                Cart cart = new Cart();
                List<CartItem> cartItems = new List<CartItem>();
                var carts = await _context.Carts.ToListAsync();
                int newId = carts.Count + 1;

                cartItems.Add(new CartItem { Dish = dish });

                cart.CartId = newId;
                cart.Items = cartItems;

                HttpContext.Session.SetInt32("Cart", newId);

                _context.Add(cart);
                await _context.SaveChangesAsync();

            }
            else
            {
                var cartId = HttpContext.Session.GetInt32("Cart");
                Cart cart = await _context.Carts.Include(x => x.Items).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartId == cartId);

                var cartItem = await _context.CartItems.ToListAsync();
                int newId = cartItem.Count + 1;

                cart.Items.Add(new CartItem { CartItemId = newId, Dish = dish });

                _context.Update(cart);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Index", "Dishes");
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cart/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cart/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cart/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}