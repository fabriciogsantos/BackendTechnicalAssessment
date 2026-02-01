using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.Entities;
using System.Linq.Expressions;

namespace Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;

public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    IEnumerable<TEntity> GetAll(Pagination pagination);
	IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate);
	TEntity? GetById(int id);
	bool Create(TEntity item);
    bool Update(TEntity item);
    bool Delete(TEntity item);
	int GetNextId();
}
