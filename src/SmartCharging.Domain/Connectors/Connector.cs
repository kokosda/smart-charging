using SmartCharging.Core.Domain;
using SmartCharging.Core.Interfaces;
using SmartCharging.Domain.Groups;
using System.Threading.Tasks;

namespace SmartCharging.Domain.Connectors
{
	public sealed class Connector : EntityBase<int>
	{
		private decimal maxCurrentInAmps;

		public int LineNo { get; init; }
		public int ChargeStationId { get; init; }
		public decimal MaxCurrentInAmps 
		{ 
			get => maxCurrentInAmps; 
			init => maxCurrentInAmps = value; 
		}

		public async Task<IResponseContainer> UpdateMaxCurrrentInAmps(decimal value, IGroupRepository groupRepository)
		{
			var result = await new UpdateConnectorMaxCurrentSpecification(value, groupRepository).IsSatisfiedBy(this);

			if (result.IsSuccess)
				maxCurrentInAmps = value;

			return result;
		}

		public string GetNumericId()
		{
			var result = $"{ChargeStationId}-{LineNo}";
			return result;
		}
	}
}
