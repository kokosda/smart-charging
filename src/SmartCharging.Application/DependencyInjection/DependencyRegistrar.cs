using Microsoft.Extensions.DependencyInjection;
using SmartCharging.Application.ChargeStations;
using SmartCharging.Application.Connectors;
using SmartCharging.Application.GeneralRequests;
using SmartCharging.Application.Groups;
using SmartCharging.Domain.Connectors.Factories;
using SmartCharging.Domain.Connectors.Strategies;

namespace SmartCharging.Application.DependencyInjection
{
	public static class DependencyRegistrar
	{
		public static IServiceCollection AddApplicationLevelServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IUpdateMaxCurrentConnectorHandler, UpdateMaxCurrentConnectorHandler>();
			serviceCollection.AddSingleton<ICreateChargeStationHandler, CreateChargeStationHandler>();
			serviceCollection.AddSingleton<ICreateGroupHandler, CreateGroupHandler>();
			serviceCollection.AddSingleton<IDeleteGroupHandler, DeleteGroupHandler>();
			serviceCollection.AddSingleton<ICreateConnectorHandler, CreateConnectorHandler>();
			serviceCollection.AddSingleton(typeof(IGetIntIdEntityHandler<,>), typeof(GetIntIdEntityHandler<,>));
			serviceCollection.AddSingleton<IConnectorFactory, ConnectorFactory>();
			serviceCollection.AddSingleton<IConnectorAllocationStrategy, AllocationThroughMinNumberExclusionStrategy>();
			return serviceCollection;
		}
	}
}
