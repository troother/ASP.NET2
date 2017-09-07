using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Models
{
    public class CartItemIngredient
    {
        public int CartItemIngredientId { get; set; }
        public Guid CartItemId { get; set; }
        public CartItem CartItem { get; set; }
        [DisplayName("Ingredient")]
        public string IngredientName { get; set; }
        public int CartItemIngredientPrice { get; set; }
        public bool Selected { get; set; }
    }
}