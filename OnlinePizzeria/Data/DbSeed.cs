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
                var pizza = new Category() { CategoryId = 1, CategoryName = "Pizza" };
                var pasta = new Category() { CategoryId = 2, CategoryName = "Pasta" };
                var salad = new Category() { CategoryId = 3, CategoryName = "Salad" };

                var cheese = new Ingredient { IngredientId = 1, IngredientName = "Cheese" };
                var tomato = new Ingredient { IngredientId = 2, IngredientName = "Tomato" };
                var ham = new Ingredient { IngredientId = 3, IngredientName = "Ham" };

                var capricciosa = new Dish() { Id = 1, DishName = "Capricciosa", Price = 75, Category = pizza };
                var hawaii = new Dish() { Id = 2, DishName = "Hawaii", Price = 80, Category = pizza };
                var margaritha = new Dish() { Id = 3, DishName = "Margaritha", Price = 80, Category = pizza };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };

                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaCheese);
                capricciosa.DishIngredients.Add(capricciosaTomato);
                capricciosa.DishIngredients.Add(capricciosaHam);

                context.AddRange(cheese, tomato, ham);
                context.AddRange(capricciosa, hawaii, margaritha);
                context.AddRange(pizza, pasta, salad);

                context.SaveChanges();
            }
        }
    }
}
