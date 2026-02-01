namespace Carglass.TechnicalAssessment.Backend.Api.Extensions
{
	public static class ConfigurationServicesExtension
	{
		public static IServiceCollection ConfigurationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddEndpointsApiExplorer()
					.AddSwaggerGen()
					.AddAutoMapper(typeof(BL.Module).Assembly)
					.AddHealthChecks();

			return services;
		}
	}
}
