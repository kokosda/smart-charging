using Microsoft.Extensions.DependencyInjection;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Connectors;
using SmartCharging.Domain.Groups;
using SmartCharging.Infrastructure.Database;
using SmartCharging.Infrastructure.Domain;

namespace SmartCharging.Infrastructure.DependencyInjection
{
	public static class DependencyRegistrar
	{
		public static IServiceCollection AddInfrastructureLevelServices(this IServiceCollection serviceCollection, DependencyRegistrarContext context)
		{
			serviceCollection.AddSingleton<ISqlConnectionFactory>(sp => new SqlConnectionFactory(context.ConnectionString));
			serviceCollection.AddSingleton(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
			serviceCollection.AddSingleton<IGenericRepository<ChargeStation, int>, ChargeStationRepository>();
			serviceCollection.AddSingleton<IGenericRepository<Group, int>, GroupRepository>();
			serviceCollection.AddSingleton<IConnectorRepository, ConnectorRepository>();
			serviceCollection.AddSingleton<IGroupRepository, GroupRepository>();

			return serviceCollection;
		}
	}
}
