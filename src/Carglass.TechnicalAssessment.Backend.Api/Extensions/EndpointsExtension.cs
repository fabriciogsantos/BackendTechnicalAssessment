namespace Carglass.TechnicalAssessment.Backend.Api.Extensions
{
	public static class EndpointsExtension
	{
		public static WebApplication UseCustomEndpoints(this WebApplication app, IConfiguration configuration)
		{
			app.MapControllers();
			app.MapHealthChecks("/HealthCheck/ready");
			app.MapGet("/HealthCheck/KeepAlive", () => Results.Ok()).WithTags("HealthCheck"); ;
					
			return app;
		}
	}
}
