using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OnlinePizzeria.Data;
using Microsoft.AspNetCore.Identity;
using OnlinePizzeria.Models;
using Microsoft.EntityFrameworkCore;
using OnlinePizzeria.ViewModels;
using OnlinePizzeria.Services;

namespace OnlinePizzeria.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private readonly CartService _cartService;

        List<CartItem> cartItems = new List<CartItem>();
        Cart cart = new Cart();

        public CheckOutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, CartService cartService)
        {
            _context = context;
            _userManager = userManager;
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            List<CartItem> CartItems = await _cartService.ItemsInCart();
            ViewData["ItemsInCart"] = CartItems;

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                return RedirectToAction("Index", "Dishes");
            }

            cart = await _cartService.GetCart();
            cartItems = cart.Items;

            var loggedInUser = _userManager.GetUserAsync(User).Result;

            int totalSum = OrderSum();

            var paymentItems = new CheckOutViewModel()
            {
                Dishes = cartItems,
                OrderSum = totalSum
            };

            if (loggedInUser != null)
            {
                paymentItems.CustomerName = loggedInUser.UserName;
                paymentItems.City = loggedInUser.City;
                paymentItems.Street = loggedInUser.Street;
                paymentItems.ZipCode = loggedInUser.ZipCode;                
            }

            ViewData["OrderSum"] = OrderSum();

            return View(paymentItems);
        }

        [HttpPost]
        public async Task<IActionResult> Payment(CheckOutViewModel paymentItems)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Receipt", paymentItems);
            }
            else
            {
                cart = await _cartService.GetCart();
                cartItems = cart.Items;

                ViewData["OrderSum"] = OrderSum();

                ViewData["ItemsInCart"] = cartItems;

                return View();
            }
        }

        public async Task<IActionResult> Receipt()
        {
            cart = await _cartService.GetCart();

            cartItems = cart.Items;

            ViewData["OrderSum"] = OrderSum();

            HttpContext.Session.Clear();
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return View();
        }

        public int OrderSum()
        {
            int totalSum = 0;
            foreach (var dish in cartItems)
            {
                totalSum += dish.Price;
            }

            return totalSum;
        }
    }
}