using SmartCharging.Core.Interfaces;
using SmartCharging.Core.ResponseContainers;
using System.Threading.Tasks;

namespace SmartCharging.Domain.Common
{
	public sealed record NameSpecification : ICommonSpecification<string>
	{
		private const int MIN_NAME_LENGTH = 1;
		private const int MAX_NAME_LENGTH = 127;

		public Task<IResponseContainer> IsSatisfiedBy(string name)
		{
			var result = new ResponseContainer();
			
			if (string.IsNullOrWhiteSpace(name))
			{
				result.AddErrorMessage($"String should not be empty or contain only spaces.");
				return Task.FromResult(result.AsInterface());
			}

			if (name.Length < MIN_NAME_LENGTH)
				result.AddErrorMessage($"String length is less than minimum allowed {MIN_NAME_LENGTH}.");
			if (name.Length > MAX_NAME_LENGTH)
				result.AddErrorMessage($"String length exceeds maximum allowed length {MIN_NAME_LENGTH}.");

			return Task.FromResult(result.AsInterface());
		}
	}
}
