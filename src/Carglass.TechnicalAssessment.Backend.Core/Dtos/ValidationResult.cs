namespace Carglass.TechnicalAssessment.Backend.Core.Dtos
{
	public class ValidationResult
	{
		public List<ErrorResult> Errors { get; } = new();
		public ValidationResult() { }

		public ValidationResult(string error, int code=-1)
		{
			Errors.Add(new ErrorResult(error, code));
		}

		public ValidationResult(IEnumerable<string> errors)
		{
			Errors.AddRange(errors.Select(error=> new ErrorResult(error, -1)));
		}
		public ValidationResult(IEnumerable<ErrorResult> errors)
		{
			Errors.AddRange(errors);
		}
		public ValidationResult(ErrorResult error)
		{
			Errors.Add(error);
		}
		public void AddError(string error) => Errors.Add(new ErrorResult(error, -1));

		public void AddError(ErrorResult error) => Errors.Add(error);

		public void AddErrors(IEnumerable<ErrorResult> errors) => Errors.AddRange(errors);

		public void AddErrors(IEnumerable<string> errors) => Errors.AddRange(errors.Select(x => new ErrorResult(x, -1)));
				
		public bool IsValid() => !Errors.Any();

		public static ValidationResult Failed(string error) => new(error);
		public static ValidationResult Failed(string error, int code) => new(new ErrorResult(error, code));
		public static ValidationResult Failed(IEnumerable<string> errors) => new(errors);
		public static ValidationResult Failed(IEnumerable<ErrorResult> errors) => new(errors);
		public static ValidationResult Valid() => new();
	}
}
