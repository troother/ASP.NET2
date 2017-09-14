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

                var cheese = new Ingredient { IngredientId = 1, IngredientName = "Cheese", Price = 10 };
                var tomato = new Ingredient { IngredientId = 2, IngredientName = "Tomato", Price = 10 };
                var ham = new Ingredient { IngredientId = 3, IngredientName = "Ham", Price = 10 };
                var mushroom = new Ingredient { IngredientId = 4, IngredientName = "Mushroom", Price = 10 };
                var shrimp = new Ingredient { IngredientId = 5, IngredientName = "Shrimp", Price = 10 };
                var chicken = new Ingredient { IngredientId = 6, IngredientName = "Chicken", Price = 10 };
                var pepper = new Ingredient { IngredientId = 7, IngredientName = "Pepper", Price = 10 };
                var tuna = new Ingredient { IngredientId = 8, IngredientName = "Tuna", Price = 10 };
                var egg = new Ingredient { IngredientId = 9, IngredientName = "Egg", Price = 10 };
                var bacon = new Ingredient { IngredientId = 10, IngredientName = "Bacon", Price = 10 };
                var pineapple = new Ingredient { IngredientId = 11, IngredientName = "Pineapple", Price = 10 };

                var capricciosa = new Dish() { Id = 1, DishName = "Capricciosa", Price = 75, Category = pizza };
                var hawaii = new Dish() { Id = 2, DishName = "Hawaii", Price = 80, Category = pizza };
                var margaritha = new Dish() { Id = 3, DishName = "Margaritha", Price = 80, Category = pizza };

                var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
                var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };
                var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };

                var hawaiiCheese = new DishIngredient { Dish = hawaii, Ingredient = cheese };
                var hawaiiTomato = new DishIngredient { Dish = hawaii, Ingredient = tomato };
                var hawaiiHam = new DishIngredient { Dish = hawaii, Ingredient = ham };
                var hawaiiPineapple = new DishIngredient { Dish = hawaii, Ingredient = pineapple };

                var margarithaCheese = new DishIngredient { Dish = margaritha, Ingredient = cheese };
                var margarithaTomato = new DishIngredient { Dish = margaritha, Ingredient = tomato };

                capricciosa.DishIngredients = new List<DishIngredient>();
                capricciosa.DishIngredients.Add(capricciosaCheese);
                capricciosa.DishIngredients.Add(capricciosaTomato);
                capricciosa.DishIngredients.Add(capricciosaHam);

                hawaii.DishIngredients = new List<DishIngredient>();
                hawaii.DishIngredients.Add(hawaiiCheese);
                hawaii.DishIngredients.Add(hawaiiTomato);
                hawaii.DishIngredients.Add(hawaiiHam);
                hawaii.DishIngredients.Add(hawaiiPineapple);

                margaritha.DishIngredients = new List<DishIngredient>();
                margaritha.DishIngredients.Add(margarithaCheese);
                margaritha.DishIngredients.Add(margarithaTomato);

                context.AddRange(cheese, tomato, ham, mushroom, shrimp, chicken, pepper, tuna, egg, bacon, pineapple);
                context.AddRange(capricciosa, hawaii, margaritha);
                context.AddRange(pizza, pasta, salad);

                context.SaveChanges();
            }
        }
    }
}
