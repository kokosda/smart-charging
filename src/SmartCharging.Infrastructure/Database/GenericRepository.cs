using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using SmartCharging.Core.Entities;
using SmartCharging.Core.Interfaces;

namespace SmartCharging.Infrastructure.Database
{
	public class GenericRepository<T, TId> : IGenericRepository<T, TId> where T: EntityBase<TId>
	{
		private readonly ISqlConnectionFactory factory;

		public GenericRepository(ISqlConnectionFactory factory)
		{
			this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
		}

		public async Task<T> CreateAsync(EntityBase<TId> entity)
		{
			var columns = GetColumns();
			var stringOfColumns = string.Join(", ", columns);
			var stringOfParameters = string.Join(", ", columns.Select(e => "@" + e));
			var query = $"insert into {typeof(T).Name} ({stringOfColumns}) output inserted.Id values ({stringOfParameters})";

			var result = (T)await factory.GetOpenConnection().ExecuteScalarAsync(query, entity);
			return result;
		}

		public Task<T> GetAsync(TId id)
		{
			var sql = $"select * from {typeof(T).Name} where Id={id}";
			var result = factory.GetOpenConnection().QueryFirstOrDefaultAsync<T>(sql);
			return result;
		}

		public async Task UpdateAsync(EntityBase<TId> entity)
		{
			var columns = GetColumns();
			var stringOfColumns = string.Join(", ", columns.Select(e => $"{e} = @{e}"));
			var query = $"update {typeof(T).Name} set {stringOfColumns} where Id = @Id";

			await factory.GetOpenConnection().ExecuteAsync(query, entity);
		}

		public async Task DeleteAsync(TId id)
		{
			var query = $"delete from {typeof(T).Name} where Id = @Id";
			await factory.GetOpenConnection().ExecuteAsync(query);
		}

		private IList<string> GetColumns()
		{
			var result = typeof(T)
				.GetProperties()
				.Where(e => e.Name != "Id" && !e.PropertyType.GetTypeInfo().IsGenericType)
				.Select(e => e.Name)
				.ToList();
			return result;
		}
	}
}
