﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
        public List<CartItemIngredient> CartItemIngredients { get; set; }
    }
}
