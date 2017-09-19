using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlinePizzeria.Data;
using OnlinePizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Services
{
    public class CartService
    {
        List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();
        CartItem cartItem = new CartItem();
        ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Cart> AddToCart(Dish dish)
        {
            Cart cart = new Cart();
            List<CartItem> cartItems = new List<CartItem>();
            if (_session.GetInt32("Cart") == null)
            {             
                var carts = await _context.Carts.ToListAsync();
                int newId = carts.Count + 1;
                var newCartItemId = Guid.NewGuid();

                _session.SetInt32("Cart", newId);

                foreach (var item in dish.DishIngredients)
                {

                    var newCartItemIngredient = new CartItemIngredient
                    {
                        CartItem = cartItem,
                        CartItemId = newCartItemId,
                        IngredientName = item.Ingredient.IngredientName,
                        CartItemIngredientPrice = item.Ingredient.Price,
                        Selected = true
                    };

                    cartItemIngredient.Add(newCartItemIngredient);
                }

                cartItems.Add(new CartItem
                {
                    Dish = dish,
                    CartId = newId,
                    Cart = cart,
                    CartItemIngredients = cartItemIngredient,
                    CartItemId = newCartItemId,
                    Price = dish.Price
                });

                cart.CartId = newId;
                cart.Items = cartItems;

                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newCartItemId = Guid.NewGuid();
                var cartId = _session.GetInt32("Cart");
                cart = await _context.Carts
                    .Include(x => x.Items)
                    .ThenInclude(z => z.Dish)
                    .SingleOrDefaultAsync(y => y.CartId == cartId);


                foreach (var item in dish.DishIngredients)
                {

                    var newCartItemIngredient = new CartItemIngredient
                    {
                        CartItem = cartItem,
                        CartItemId = newCartItemId,
                        IngredientName = item.Ingredient.IngredientName,
                        CartItemIngredientPrice = item.Ingredient.Price,
                        Selected = true,
                        CartItemIngredientId = GenerateCartItemIngredientId()
                    };

                    cartItemIngredient.Add(newCartItemIngredient);
                }

                cartItem.CartItemId = newCartItemId;
                cartItem.Dish = dish;
                cartItem.Cart = cart;
                cartItem.CartId = cart.CartId;
                cartItem.CartItemIngredients = cartItemIngredient;
                cartItem.Price = dish.Price;

                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
            }

            return (cart);
        }
        public int GenerateCartItemIngredientId()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public async Task<List<CartItem>> ItemsInCart()
        {
            List<CartItem> cartItems = new List<CartItem>();

            if (_session.GetInt32("Cart") == null)
            {
                var carts = await _context.Carts.ToListAsync();
                var newId = _session.GetInt32("Cart");
                newId = carts.Count + 1;

                cartItems = new List<CartItem>();
            }

            else
            {
                Cart cart = new Cart();
                var cartId = _session.GetInt32("Cart");
                cart = await _context.Carts.Include(x => x.Items).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartId == cartId);
                cartItems = cart.Items;
            }

            return (cartItems);
        }

        public async Task<Cart> GetCart()
        {
            Cart cart = new Cart();
            var cartId = _session.GetInt32("Cart");            
            cart = await _context.Carts.Include(x => x.Items).ThenInclude(z => z.Dish).SingleOrDefaultAsync(y => y.CartId == cartId);
            return cart;
        }
    }
}

