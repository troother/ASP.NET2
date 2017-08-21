using Microsoft.AspNetCore.Identity;
using OnlinePizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Data
{
    public static class DbSeed
    {
        public static void Seed(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var aUser = new ApplicationUser();
            aUser.Email = "test.student@mail.com";
            aUser.UserName = "test.student@mail.com";
            var r = userManager.CreateAsync(aUser, "Pa$$word1").Result;

            var adminRole = new IdentityRole { Name = "Admin" };
            var roleResult = roleManager.CreateAsync(adminRole).Result;

            var adminUser = new ApplicationUser();
            adminUser.UserName = "admin@test.com";
            adminUser.Email = "admin@test.com";
            var adminUserResult = userManager.CreateAsync(adminUser, "Pa$$word2").Result;

            userManager.AddToRoleAsync(adminUser, "Admin");

            if (!context.Dishes.Any())
            {
                var cheese = new Ingredient { Name = "Cheese" };
                var tomatoe = new Ingredient { Name = "Tomatoe" };
                var ham = new Ingredient { Name = "Ham" };
                var capricciosa = new Dish() { Id = 1, Name = "capricciosa", Price = 75 };
                var hawaii = new Dish() { Id = 2, Name = "Hawaii", Price = 80 };
                var margaritha = new Dish() { Id = 3, Name = "Margaritha", Price = 80 };
                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
                var capricciosaTomatoe = new DishIngredient { Dish = capricciosa, Ingredient = tomatoe };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };
                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaCheese);
                capricciosa.DishIngredients.Add(capricciosaTomatoe);
                capricciosa.DishIngredients.Add(capricciosaHam);
                context.AddRange(cheese, tomatoe, ham);
                context.AddRange(capricciosa, hawaii, margaritha);
                context.SaveChanges();
            }
        }
    }
}
