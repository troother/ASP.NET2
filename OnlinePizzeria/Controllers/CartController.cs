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
        public async Task<ActionResult> AddToCart(int Id)
        {

            Dish dish = await _context.Dishes.Include(x => x.DishIngredients).ThenInclude(i => i.Ingredient).SingleOrDefaultAsync(d => d.Id == Id);
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();
            CartItem cartItem = new CartItem();

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                Cart cart = new Cart();
                List<CartItem> cartItems = new List<CartItem>();
                var carts = await _context.Carts.ToListAsync();
                int newId = carts.Count + 1;
                var newCartItemId = Guid.NewGuid();

                foreach (var item in dish.DishIngredients)
                {
                    var newCartItemIngredient = new CartItemIngredient
                    {
                        CartItem = cartItem,
                        CartItemId = newCartItemId,
                        IngredientName = item.Ingredient.IngredientName,
                        CartItemIngredientPrice = item.Ingredient.Price
                    };

                    cartItemIngredient.Add(newCartItemIngredient);
                }

                cartItems.Add(new CartItem { Dish = dish, CartId = newId, Cart = cart, CartItemIngredients = cartItemIngredient, CartItemId = newCartItemId });

                cart.CartId = newId;
                cart.Items = cartItems;

                HttpContext.Session.SetInt32("Cart", newId);

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

            }
            else
            {
                var cartID = HttpContext.Session.GetInt32("Cart");
                Cart cart = await _context.Carts.Include(x => x.Items).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartId == cartID);

                var newCartItemId = Guid.NewGuid();

                foreach (var item in dish.DishIngredients)
                {
                    var newCartItemIngredient = new CartItemIngredient
                    {
                        CartItem = cartItem,
                        CartItemId = newCartItemId,
                        IngredientName = item.Ingredient.IngredientName,
                        CartItemIngredientPrice = item.Ingredient.Price
                    };

                    cartItemIngredient.Add(newCartItemIngredient);
                }

                cart.Items.Add(new CartItem { CartItemId = newCartItemId, Dish = dish, Cart = cart, CartId = cart.CartId, CartItemIngredients = cartItemIngredient });

                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Index", "Dishes");
        }

        // GET: Cart/Delete
        public async Task<ActionResult> Delete(Guid id)
        {
            var cartID = HttpContext.Session.GetInt32("Cart");
            Cart cart = await _context.Carts.Include(x => x.Items).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartId == cartID);
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartItemId == id);

            if (cartItem != null)
            {
                //cartItem.CartItemIngredients.RemoveAll(ci => ci.CartItemID == cartItem.CartItemID);
                cart.Items.RemoveAll(ci => ci.CartItemId == cartItem.CartItemId);

                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Cart");
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
        public ActionResult Edit(Guid id)
        {
            return View();
        }

        // POST: Cart/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Guid id, IFormCollection collection)
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