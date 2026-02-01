namespace Carglass.TechnicalAssessment.Backend.Entities;

public class Product : Entity
{
    public string Name { get; set; }
    public int Type { get; set; }
    public double NumTerminal { get; set; }
    public DateTime SoldAt { get; set; }
}