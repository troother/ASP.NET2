using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using OnlinePizzeria.Services;
using OnlinePizzeria.Models;

namespace OnlinePizzeriaTest
{
    public class CalculateCartTotalServiceTest
    {
        private readonly CalculateCartTotalService _CalculateCartTotalService;

        public CalculateCartTotalServiceTest()
        {
            _CalculateCartTotalService = new CalculateCartTotalService();
        }

        [Fact]
        public void CartSumTest()
        {
            var pizza = new Category() { CategoryId = 1, CategoryName = "Pizza" };

            var cheese = new Ingredient { IngredientId = 1, IngredientName = "Cheese", Price = 10 };
            var tomato = new Ingredient { IngredientId = 2, IngredientName = "Tomato", Price = 10 };
            var ham = new Ingredient { IngredientId = 3, IngredientName = "Ham", Price = 10 };
            var pineapple = new Ingredient { IngredientId = 11, IngredientName = "Pineapple", Price = 10 };

            var capricciosa = new Dish() { Id = 1, DishName = "Capricciosa", Price = 75, Category = pizza };

            var capricciosaCheese = new DishIngredient { Dish = capricciosa, Ingredient = cheese };
            var capricciosaTomato = new DishIngredient { Dish = capricciosa, Ingredient = tomato };
            var capricciosaHam = new DishIngredient { Dish = capricciosa, Ingredient = ham };

            capricciosa.DishIngredients = new List<DishIngredient>();
            capricciosa.DishIngredients.Add(capricciosaCheese);
            capricciosa.DishIngredients.Add(capricciosaTomato);
            capricciosa.DishIngredients.Add(capricciosaHam);

            Cart cart = new Cart();
            
            List<CartItem> cartItems = new List<CartItem>();
            var newCartItemID = Guid.NewGuid();
            List<CartItemIngredient> cartItemIngredient = new List<CartItemIngredient>();
            CartItem cartItem = new CartItem();

            foreach (var item in capricciosa.DishIngredients)
            {
                var newCartItemIngredient = new CartItemIngredient
                {
                    CartItem = cartItem,
                    CartItemId = newCartItemID,
                    IngredientName = item.Ingredient.IngredientName,
                    CartItemIngredientPrice = item.Ingredient.Price,
                    Selected = true,
                    CartItemIngredientId = GenerateCartItemIngredientId()
                };

                cartItemIngredient.Add(newCartItemIngredient);
            }

            cartItem.CartItemId = newCartItemID;
            cartItem.Dish = capricciosa;
            cartItem.Cart = cart;
            cartItem.CartId = cart.CartId;
            cartItem.CartItemIngredients = cartItemIngredient;
            cartItem.Price = capricciosa.Price;

            cartItems.Add(cartItem);

            cart.Items = cartItems;

            var actual = capricciosa.Price;

            var result = _CalculateCartTotalService.CartTotal(cart);

            Assert.Equal(actual, result);
        }

        public int GenerateCartItemIngredientId()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }
    }
}
