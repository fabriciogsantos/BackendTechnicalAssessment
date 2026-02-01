namespace Carglass.TechnicalAssessment.Backend.Entities;

public class Client : Entity
{
    public string DocType { get; set; }
    public string DocNum { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
}