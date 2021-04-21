using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace SmartCharging.Infrastructure.Tests
{
	[TestFixture]
	internal abstract class BaseTest
	{
		private IConfiguration config;

		protected IConfiguration Configuration => config;

		[SetUp]
		public void Setup()
		{
			config = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
		}
	}
}
