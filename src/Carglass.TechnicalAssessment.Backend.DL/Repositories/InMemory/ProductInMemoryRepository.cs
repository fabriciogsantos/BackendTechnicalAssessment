using Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;
using Carglass.TechnicalAssessment.Backend.DL.Repositories.Core;
using Carglass.TechnicalAssessment.Backend.Entities;
using Carglass.TechnicalAssessment.Backend.Seeds.Products;


namespace Carglass.TechnicalAssessment.Backend.DL.Repositories.InMemory;

public class ProductInMemoryRepository : Repository<Product>, IProductInMemoryRepository
{
    public override HashSet<Product> Context { get; protected set; } = ProductSeed.Generate(200);
}
