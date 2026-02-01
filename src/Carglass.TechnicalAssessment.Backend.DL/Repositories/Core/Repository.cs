using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Carglass.TechnicalAssessment.Backend.DL.Interfaces.Repositories;
using Carglass.TechnicalAssessment.Backend.Entities;
using System.Linq.Expressions;

namespace Carglass.TechnicalAssessment.Backend.DL.Repositories.Core
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity,new()
    {
        public abstract HashSet<TEntity> Context { get; protected set; }
        public IEnumerable<TEntity> GetAll(Pagination pagination)
        {
			pagination ??= new Pagination();
			return  Context.OrderBy(p=>p.Id).Skip(pagination.TakeSkip()).Take(pagination.Take);
        }

        public TEntity? GetById(int id)
        {
            return Context.TryGetValue(new TEntity { Id = id }, out var entity) ? entity : null;
        }
        
        public bool Create(TEntity item)
        {
            return Context.Add(item);
        }

        public bool Update(TEntity item)
        {
            var existing = Context.FirstOrDefault(c => c.Id == item.Id);
            if (existing is null) return false;

            return Context.Remove(existing) && Context.Add(item);
        }

        public bool Delete(TEntity item)
        {
            var toDeleteItem = Context.SingleOrDefault(c => c.Id == item.Id);
            if (toDeleteItem is null) return false;

            return Context.Remove(toDeleteItem);
        }

		public IEnumerable<TEntity> Search(Expression<Func<TEntity, bool>> predicate)
		{
			return Context.AsQueryable().Where(predicate);
		}

		public void Dispose()
        {
          GC.SuppressFinalize(this);
        }

		public int GetNextId()
		{
			return Context.Count == 0 ? 1 : Context.Max(e => e.Id) + 1;
		}
	}
}
