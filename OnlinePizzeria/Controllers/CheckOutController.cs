﻿using System;
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

namespace OnlinePizzeria.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        List<CartItem> cartItems = new List<CartItem>();
        Cart cart = new Cart();

        public CheckOutController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {

            if (HttpContext.Session.GetInt32("Cart") == null)
            {
                return RedirectToAction("Index", "Dishes");
            }

            var cartId = HttpContext.Session.GetInt32("Cart");
            cart = await _context.Carts.Include(x => x.Items).SingleOrDefaultAsync(y => y.CartId == cartId);
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

            return View(paymentItems);
        }

        [HttpPost]
        public IActionResult Payment(CheckOutViewModel paymentItems)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Receipt", paymentItems);
            }

            return View(paymentItems);
        }

        public async Task<IActionResult> Receipt()
        {
            var cartId = HttpContext.Session.GetInt32("Cart");
            cart = await _context.Carts.Include(x => x.Items).SingleOrDefaultAsync(y => y.CartId == cartId);
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