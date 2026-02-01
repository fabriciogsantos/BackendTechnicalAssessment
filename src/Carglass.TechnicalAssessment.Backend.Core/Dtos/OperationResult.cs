namespace Carglass.TechnicalAssessment.Backend.Core.Dtos
{
	public class OperationResult<TResult> : OperationResult
	{
		public OperationResult() : base() { }
		public TResult? Result { get; set; }

	}

	public class OperationResult
	{
		public OperationResult()
		{
			Errors = new List<ErrorResult>();
		}
		public List<ErrorResult> Errors { get; }
		public bool HasErrors => Errors.Any();
		public bool HasExceptions => Errors.Exists(x => x.Exception != null);
	}
}
