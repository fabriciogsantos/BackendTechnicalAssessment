using FluentValidation;
using System.Text.RegularExpressions;

namespace Carglass.TechnicalAssessment.Backend.Dtos.Extensions
{
    public static class VallidatorExtensions
    {
		private static readonly Regex NifRegex =new Regex(@"^\d{8}[A-Za-z]$", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));
		public static IRuleBuilderOptions<T, string> MustValidNumberDocument<T>(this IRuleBuilder<T,string> ruleBuilder){
			

			return ruleBuilder.Must(numDoc => NifRegex.IsMatch(numDoc.Trim())).WithMessage("'{PropertyName}' el número del documento NIF no es valido");
		}
	}
}
