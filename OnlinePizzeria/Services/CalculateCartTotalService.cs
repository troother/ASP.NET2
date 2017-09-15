using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlinePizzeria.Data;
using OnlinePizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlinePizzeria.Services
{
    public class CalculateCartTotalService
    {
        public int CartTotal(Cart cart)
        {
            int totalSum = 0;
            foreach (var dish in cart.Items)
            {
                totalSum += dish.Price;
            }

            return totalSum;
        }
    }
}
