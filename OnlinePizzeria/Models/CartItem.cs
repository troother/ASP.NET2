using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Models
{
    public class CartItem
    {
        public Guid CartItemId { get; set; }
        public Cart Cart { get; set; }
        public int CartId { get; set; }
        public Dish Dish { get; set; }
        public List<CartItemIngredient> CartItemIngredients { get; set; }
    }
}
