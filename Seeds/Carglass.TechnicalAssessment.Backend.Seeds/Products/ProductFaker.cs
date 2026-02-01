using Bogus;
using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.Seeds.Products
{
    public static class ProductFaker
    {
        public static Faker<Product> Create()
        {
            int id = 1;

            return new Faker<Product>("es")
                .RuleFor(c => c.Id, _ => id++)
                .RuleFor(c => c.Name, f => f.Commerce.ProductName())
                .RuleFor(c => c.Type, f => f.Random.Int(1, 99))
                .RuleFor(c => c.NumTerminal, f => f.Random.Double())
                .RuleFor(c => c.SoldAt, f => f.Date.Past(2));
        }
    }
}
