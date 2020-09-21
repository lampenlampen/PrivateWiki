using System;
using System.Drawing;
using System.IO;
using System.Linq;
using NodaTime;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.StorageBackendService.SQLite;
using Xunit;
using Label = PrivateWiki.DataModels.Pages.Label;
using FluentAssertions;

namespace PrivateWiki.Test.Services.StorageBackendService.SQLite
{
	public class LabelBackendTest : IDisposable
	{
		private readonly string _dbPath;
		private readonly SqLiteBackend _backend;

		public LabelBackendTest()
		{
			_dbPath = Guid.NewGuid().ToString();
			_backend = new SqLiteBackend(new SqLiteStorageOptions(_dbPath), SystemClock.Instance);
		}

		[Fact]
		public async void InsertGetLabelsTest()
		{
			var label = Label.GetTestData()[0];

			await _backend.InsertLabelAsync(label);
			
			var actualLabel = (await _backend.GetAllLabelsAsync()).First();

			actualLabel.Should().BeEquivalentTo(label, options => options
				.Using<Color>(ctx =>
				{
					var actual = ctx.Subject;
					var expected = ctx.Expectation;

					actual.A.Should().Be(expected.A);
					actual.B.Should().Be(expected.B);
					actual.G.Should().Be(expected.G);
					actual.R.Should().Be(expected.R);
				})
				.WhenTypeIs<Color>());
		}

		public void Dispose()
		{
			File.Delete(Path.GetFullPath($"{_dbPath}.db"));
		}
	}
}