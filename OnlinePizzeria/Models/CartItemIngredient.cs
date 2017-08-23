﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Models
{
    public class CartItemIngredient
    {
        public int CartItemId { get; set; }
        public CartItem CartItem { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public bool Enabled { get; set; }
    }
}
