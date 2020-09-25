using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService.SQLite;
using Xunit;
using Path = System.IO.Path;

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

			actualLabel.Should().BeEquivalentTo(label);
		}

		public void Dispose()
		{
			File.Delete(Path.GetFullPath($"{_dbPath}.db"));
		}
	}
}