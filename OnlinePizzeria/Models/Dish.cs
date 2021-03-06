﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Models
{
    public class Dish
    {
        public int Id { get; set; }
        public string DishName { get; set; }
        public int Price { get; set; }
        public List<DishIngredient> DishIngredients { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
