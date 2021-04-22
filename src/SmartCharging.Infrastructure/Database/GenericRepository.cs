using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SmartCharging.Core;
using SmartCharging.Core.Entities;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Infrastructure.Database
{
	public sealed class GenericRepository<T, TId> : IGenericRepository<T, TId> where T: EntityBase<TId>
	{
		private readonly ISqlConnectionFactory factory;

		public GenericRepository(ISqlConnectionFactory factory)
		{
			factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		public Task<T> CreateAsync(EntityBase<TId> entity)
		{
			throw new NotImplementedException();
		}

		public Task<T> GetAsync(TId id)
		{
			var sql = $"select * from {typeof(T).Name} where Id={id}";
			var result = factory.GetOpenConnection().QueryFirstOrDefaultAsync<T>(sql);
			return result;
		}

		public Task UpdateAsync(EntityBase<TId> entity)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(TId id)
		{
			throw new NotImplementedException();
		}

		private IEnumerable<string> GetColumns()
		{
			var result = typeof(T)
				.GetProperties()
				.Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
				.Select(e => e.Name);
			return result;
		}
	}
}
