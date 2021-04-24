using Microsoft.Extensions.DependencyInjection;
using SmartCharging.Application.ChargeStations;
using SmartCharging.Application.Connectors;
using SmartCharging.Application.GeneralRequests;
using SmartCharging.Application.Groups;

namespace SmartCharging.Application.DependencyInjection
{
	public static class DependencyRegistrar
	{
		public static IServiceCollection AddApplicationLevelServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IUpdateMaxCurrentConnectorHandler, UpdateMaxCurrentConnectorHandler>();
			serviceCollection.AddSingleton<ICreateChargeStationHandler, CreateChargeStationHandler>();
			serviceCollection.AddSingleton<ICreateGroupHandler, CreateGroupHandler>();
			serviceCollection.AddSingleton(typeof(IGetIntIdEntityHandler<,>), typeof(GetIntIdEntityHandler<,>));
			return serviceCollection;
		}
	}
}
