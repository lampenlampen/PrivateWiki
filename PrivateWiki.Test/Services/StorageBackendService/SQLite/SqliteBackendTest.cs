using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.AppSettingsService.BackendSettings;
using PrivateWiki.Services.Backends;
using PrivateWiki.Services.Backends.Sqlite;
using Xunit;
using Path = System.IO.Path;

namespace PrivateWiki.Test.Services.StorageBackendService.SQLite
{
	public class LabelBackendTest : IDisposable
	{
		private readonly string _dbPath;
		private readonly ILabelBackend _backend;

		public LabelBackendTest()
		{
			IBackendSettingsService settings = new BackendSettingsTestService();

			_dbPath = settings.GetSqliteBackendPath();

			var conv1 = new DbReaderToLabelConverter();

			_backend = new LabelSqliteBackend(settings, conv1, new DbReaderToLabelsConverter(conv1));
		}

		[Fact]
		public async void InsertGetLabelsTest()
		{
			var label = new Label("testKey1", "Description 1", System.Drawing.Color.Red.ToColor());

			await _backend.InsertLabelAsync(label);

			var actualLabel = (await _backend.GetAllLabelsAsync()).First();

			actualLabel.Should().BeEquivalentTo(label);
		}

		public void Dispose()
		{
			var path = Path.GetFullPath(_dbPath);

			File.Delete(path);
		}
	}
}