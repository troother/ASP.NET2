using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlinePizzeria.Models;
using OnlinePizzeria.Data;
using Microsoft.EntityFrameworkCore;
using OnlinePizzeria.Services;

namespace OnlinePizzeria.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
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
            var Cart = await _cartService.AddToCart(dish);

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
        public async Task<ActionResult> Edit(Guid id)
        {
            CartItem cartItem = _context.CartItems.Include(x => x.CartItemIngredients).Include(z => z.Dish).SingleOrDefault(y => y.CartItemId == id);
            var extra = _context.Ingredients.Where(x => !cartItem.CartItemIngredients.Any(s => s.IngredientName.Equals(x.IngredientName))).ToList();
            var extraIngredients = extra.Select(x => new CartItemIngredient() { CartItemIngredientId = GenerateRandomCartItemIngredientId(), CartItemId = Guid.NewGuid(), IngredientName = x.IngredientName, Selected = false, CartItemIngredientPrice = x.Price }).ToList();

            cartItem.ExtraCartItemIngredients = extraIngredients;

            _context.Update(cartItem);
            await _context.SaveChangesAsync();


            return View(cartItem);
        }

        // POST: Cart/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(CartItem model)
        {
            CartItem cartItem = _context.CartItems.Include(x => x.CartItemIngredients).Include(i => i.ExtraCartItemIngredients).Include(z => z.Dish).SingleOrDefault(y => y.CartItemId == model.CartItemId);
            List<CartItemIngredient> clearExtraIngredients = new List<CartItemIngredient>();

            if (model.ExtraCartItemIngredients == null)
            {
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                foreach (var ingredient in model.ExtraCartItemIngredients)
                {
                    if (ingredient.Selected && !cartItem.CartItemIngredients.Any(x => x.CartItemIngredientId.Equals(ingredient.CartItemIngredientId)))
                    {
                        CartItemIngredient newIngredient = _context.CartItemIngredients.SingleOrDefault(s => s.CartItemIngredientId == ingredient.CartItemIngredientId);
                        var newCartItemIngredient = new CartItemIngredient
                        {
                            CartItem = cartItem,
                            CartItemId = cartItem.CartItemId,
                            IngredientName = newIngredient.IngredientName,
                            CartItemIngredientPrice = newIngredient.CartItemIngredientPrice,
                            Selected = true,
                            CartItemIngredientId = GenerateRandomCartItemIngredientId()
                        };

                        cartItem.CartItemIngredients.Add(newCartItemIngredient);
                        cartItem.Price = cartItem.Price + newIngredient.CartItemIngredientPrice;
                        cartItem.ExtraCartItemIngredients = clearExtraIngredients;
                    }
                }
            }


            foreach (var orginalIngredient in model.CartItemIngredients)
            {
                CartItemIngredient ingredientToRemove = _context.CartItemIngredients.SingleOrDefault(s => s.CartItemIngredientId == orginalIngredient.CartItemIngredientId);

                if (!orginalIngredient.Selected && cartItem.CartItemIngredients.Any(x => x.CartItemIngredientId.Equals(orginalIngredient.CartItemIngredientId)))
                {
                    cartItem.CartItemIngredients.RemoveAll(x => x.CartItemIngredientId.Equals(orginalIngredient.CartItemIngredientId));
                    cartItem.Price = cartItem.Price - ingredientToRemove.CartItemIngredientPrice;
                    cartItem.ExtraCartItemIngredients = clearExtraIngredients;
                }

            }

            _context.Update(cartItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index","Cart");
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

        public int GenerateRandomCartItemIngredientId()
        {
            int _min = 1000;
            int _max = 9999;
            Random _random = new Random();
            return _random.Next(_min, _max);
        }
    }
}