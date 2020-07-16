using System.IO;
using FluentResults;
using Xunit;
using Xunit.Abstractions;

namespace PrivateWiki.Test
{
	public class FluentResultLoggerTest
	{
		private readonly ITestOutputHelper output;

		public FluentResultLoggerTest(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Fact]
		public void FluentResultLoggerTest1()
		{
			var logger = new FluentResultXUnitLogger(output);
			Result.Setup(cfg => cfg.Logger = logger);

			Result<int> result;
			try
			{
				throw new FileNotFoundException();
			}
			catch (FileNotFoundException e)
			{
				result = Result.Fail<int>(new FileNotFoundError().CausedBy(new FileNotFoundException()))
					.WithError(new ArgumentError());
				throw;
			}

			result.Log();
		}
	}

	public class FluentResultXUnitLogger : IResultLogger
	{
		private readonly ITestOutputHelper output;

		public FluentResultXUnitLogger(ITestOutputHelper output)
		{
			this.output = output;
		}

		public void Log(string context, ResultBase result)
		{
			output.WriteLine(result.ToString());
		}
	}
}