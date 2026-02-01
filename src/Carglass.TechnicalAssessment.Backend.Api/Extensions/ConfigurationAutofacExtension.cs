using Autofac.Extensions.DependencyInjection;
using Autofac;

namespace Carglass.TechnicalAssessment.Backend.Api.Extensions
{
	public static class ConfigurationAutofacExtension
	{
		public static ConfigureHostBuilder ConfigurationAutofac(this ConfigureHostBuilder host)
		{
			host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
			host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
			{
				containerBuilder
					.RegisterModule<BL.Module>()
					.RegisterModule<DL.Module>()
					.RegisterModule<Dtos.Module>();
			});
			return host;
		}
	}
}
