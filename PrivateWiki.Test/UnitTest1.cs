using System;
using System.Threading.Tasks;
using PrivateWiki.Core;
using PrivateWiki.Dependencies.Sentry;
using Sentry;
using Xunit;
using Xunit.Abstractions;

namespace PrivateWiki.Test
{
	public class UnitTest1
	{
		private readonly ITestOutputHelper output;

		public UnitTest1(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Fact]
		public async void Main()
		{
			new RegisterSentryCmdHandler().Handle(Null.Instance);

			await CallThrow();
		}

		private async Task CallThrow()
		{
			try
			{
				ThrowEx();
			}
			catch (Exception e)
			{
				SentrySdk.CaptureException(e);
				await SentrySdk.FlushAsync(TimeSpan.FromSeconds(2));
			}
		}

		private void ThrowEx()
		{
			throw new ArithmeticException("blubtest");
		}
	}
}