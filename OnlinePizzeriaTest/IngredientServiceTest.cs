using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlinePizzeria.Data;
using System;
using Xunit;

namespace OnlinePizzeriaTest
{
    public class IngredientServiceTest
    {
        public IngredientServiceTest()
        {
            var efServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(b =>
            b.UseInMemoryDatabase("PizzaDatabas")
            .UseInternalServiceProvider(efServiceProvider));
            services.AddTransient<IngredientService>();

            _serviceProvider = services.BuildServiceProvider();

        }
        [Fact]
        public void AllAreSorted()
        {
            var _ingredients = _serviceProvider.GetService<IngredientService>();
            var ings = _ingredients.All();

            Assert.Equal(2, 0);
        }
    }
}
