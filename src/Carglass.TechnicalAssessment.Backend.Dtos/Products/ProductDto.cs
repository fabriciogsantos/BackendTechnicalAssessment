namespace Carglass.TechnicalAssessment.Backend.Dtos.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Type { get; set; }
    public double NumTerminal { get; set; }
    public DateTime SoldAt { get; set; }
}
