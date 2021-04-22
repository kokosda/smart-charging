using Microsoft.Extensions.DependencyInjection;
using SmartCharging.Application.Connectors;

namespace SmartCharging.Application.DependencyInjection
{
	public static class DependencyRegistrar
	{
		public static IServiceCollection AddApplicationLevelServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IUpdateConnectorHandler, UpdateConnectorHandler>();
			return serviceCollection;
		}
	}
}
