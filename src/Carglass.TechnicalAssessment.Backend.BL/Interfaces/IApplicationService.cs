using Carglass.TechnicalAssessment.Backend.Core.Dtos;

namespace Carglass.TechnicalAssessment.Backend.BL.Interfaces;

public interface IApplicationService<TDto>
{
	OperationResult<IEnumerable<TDto>> GetAll(Pagination pagination);
	OperationResult<TDto> GetById(int id);
	OperationResult<TDto> Create(TDto product);
	OperationResult Update(TDto product);
	OperationResult Delete(int id);
}
