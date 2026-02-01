using Bogus;
using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.Seeds.Products
{
    public static class ProductSeed
    {
        public static HashSet<Product> Generate(int count = 10)
        {
            Randomizer.Seed = new Random(1234);
            return new HashSet<Product>(ProductFaker.Create().Generate(count));
        }
    }

}
