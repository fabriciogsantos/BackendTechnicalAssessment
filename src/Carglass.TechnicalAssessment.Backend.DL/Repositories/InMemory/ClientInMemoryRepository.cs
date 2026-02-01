using Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;
using Carglass.TechnicalAssessment.Backend.DL.Repositories.Core;
using Carglass.TechnicalAssessment.Backend.Entities;
using Carglass.TechnicalAssessment.Backend.Seeds.Clients;

namespace Carglass.TechnicalAssessment.Backend.DL.Repositories.InMemory;

public class ClientInMemoryRepository : Repository<Client>, IClientInMemoryRepository
{
    public override HashSet<Client> Context { get; protected set; } = ClientSeed.Generate(200);
}
