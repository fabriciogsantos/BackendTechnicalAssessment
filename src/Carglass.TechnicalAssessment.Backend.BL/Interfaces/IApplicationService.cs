using Carglass.TechnicalAssessment.Backend.Core.Dtos;

namespace Carglass.TechnicalAssessment.Backend.BL.Interfaces;

public interface IApplicationService<TDto>
{
	OperationResult<IEnumerable<TDto>> GetAll(Pagination pagination);
	OperationResult<TDto> GetById(int id);
	OperationResult<TDto> Create(TDto dto);
	OperationResult Update(TDto dto);
	OperationResult Delete(int id);
}
