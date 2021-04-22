using SmartCharging.Core;
using SmartCharging.Core.Entities;
using SmartCharging.Domain.ChargeStations;
using SmartCharging.Domain.Exceptions;

namespace SmartCharging.Domain.Connectors
{
	public sealed class Connector : EntityBase<int>
	{
		private decimal maxCurrentInAmps;

		public int LineNo { get; init; }
		public ChargeStation ChargeStation { get; init; }
		public decimal MaxCurrentInAmps 
		{ 
			get => maxCurrentInAmps; 
			init => maxCurrentInAmps = value; 
		}

		public void UpdateMaxCurrrentInAmps(decimal value)
		{
			if (value <= 0)
				throw new BusinessRuleException($"Current can not be less than or equal to 0. Provided value is {value}.");

			maxCurrentInAmps = value;
		}

		public string GetNumericId()
		{
			var result = $"{ChargeStation.Id}-{LineNo}";
			return result;
		}
	}
}
