using Carglass.TechnicalAssessment.Backend.Core.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carglass.TechnicalAssessment.Backend.Core.Extensions
{
	public static class OperationResultExtensions
	{
		public static OperationResult<TResult> AddResult<TResult>(this OperationResult<TResult> operationResult,TResult result)
		{
			if (operationResult == null)
				throw new ArgumentNullException(nameof(operationResult),"builderContext cannot be null");
			operationResult.Result = result;
			return operationResult;
		}
		
		public static OperationResult<TResult> AddError<TResult>(this OperationResult<TResult> operationResult,string errorMessage,int errorCode,	Exception exception = null)
		{
			if (operationResult == null)
				throw new ArgumentNullException(nameof(operationResult),"builderContext cannot be null");
			operationResult.Errors.Add(new ErrorResult(errorMessage, errorCode, exception));
			return operationResult;
		}

		public static OperationResult AddError(this OperationResult operationResult,string errorMessage, int errorCode, Exception exception = null)
		{
			if (operationResult == null)
				throw new ArgumentNullException(nameof(operationResult),"builderContext cannot be null");
			operationResult.Errors.Add(new ErrorResult(errorMessage, errorCode, exception));
			return operationResult;
		}
        
		public static OperationResult<TResult> AddErrors<TResult>(this OperationResult<TResult> operationResult, IEnumerable<ErrorResult> validationErrors)
		{
			if (operationResult == null)
				throw new ArgumentNullException(nameof(operationResult),"builderContext cannot be null");
			
            operationResult.Errors.AddRange(validationErrors);
			return operationResult;
		}

		public static OperationResult AddErrors(this OperationResult operationResult, IEnumerable<ErrorResult> validationErrors)
		{
			if (operationResult == null)
				throw new ArgumentNullException(nameof(operationResult),"builderContext cannot be null");

			operationResult.Errors.AddRange(validationErrors);
			return operationResult;
		}
        
		public static IActionResult ToActionResult<TResult>(this OperationResult<TResult> operationResult,ControllerBase controller)
		{
			if (operationResult.Result == null && !operationResult.HasErrors)
				return controller.NoContent();

			if (!operationResult.HasErrors)
				return controller.Ok(operationResult.Result);

			return GetResultErrors(operationResult, controller);
		}

		public static IActionResult ToActionResult(this OperationResult operationResult,	ControllerBase controller)
		{
			if (operationResult == null)
				return controller.NoContent();

			if (!operationResult.HasErrors)
				return controller.Ok();

			return GetResultErrors(operationResult, controller);

		}
		
		private static ActionResult GetResultErrors(OperationResult operationResult, ControllerBase controller)
		{
			var problem = new ValidationProblemDetails
			{
				Instance = controller.HttpContext.Request.Path,
				Title = "Unexpected error",
				Detail = "The request could not be processed."
			};

			if (operationResult.HasExceptions)
			{
				problem.Status = StatusCodes.Status500InternalServerError;
				return controller.StatusCode(problem.Status.Value, problem);
			}

			problem.Status = StatusCodes.Status400BadRequest;

			problem.Errors.Add("errors",
				operationResult.Errors
				.Select(e => e.Message)
				.ToArray());

			return controller.StatusCode(problem.Status.Value, problem);
		}
	}
}
