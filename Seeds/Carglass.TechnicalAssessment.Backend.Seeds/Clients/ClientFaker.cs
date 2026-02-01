using Bogus;
using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.Seeds.Clients
{
    public static class ClientFaker
    {
        private const string Letters = "TRWAGMYFPDXBNJZSQVHLCKE";
        private static readonly HashSet<string> _generatedNifs = new();
        public static Faker<Client> Create()
        {
            int id = 1;

            return new Faker<Client>("es")
                .RuleFor(c => c.Id, _ => id++)
                .RuleFor(c => c.DocType, _ => "nif")
                .RuleFor(c => c.DocNum, GenerateNif)
                .RuleFor(c => c.Name, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, (f, c) =>
                    f.Internet.Email(c.Name, c.LastName))
                .RuleFor(c => c.Phone, f => f.Random.ReplaceNumbers("6########"));
        }

        private static string GenerateNif(Faker fake)
        {
           string nif;
            do
            {
                var number = fake.Random.Number(10000000, 99999999);
                var letter = Letters[number % 23];
                nif = $"{number}{letter}";
            }
            while (!_generatedNifs.Add(nif)); 

            return nif;
        }
    }
}
