using Bogus;
using Carglass.TechnicalAssessment.Backend.Entities;

namespace Carglass.TechnicalAssessment.Backend.Seeds.Clients
{
    public static class ClientSeed
    {
        public static HashSet<Client> Generate(int count = 10)
        {
            Randomizer.Seed = new Random(1234);
            return new HashSet<Client>(ClientFaker.Create().Generate(count));
        }
    }

}
