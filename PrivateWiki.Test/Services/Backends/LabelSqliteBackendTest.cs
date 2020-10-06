using System;
using System.IO;
using FluentAssertions;
using PrivateWiki.Services.Backends;
using PrivateWiki.Services.Backends.Sqlite;
using PrivateWiki.Test.Services.StorageBackendService.SQLite;
using Xunit;

namespace PrivateWiki.Test.Services.Backends
{
	public class LabelSqliteBackendTest : IDisposable
	{
		private readonly string _path;
		private readonly ILabelBackend _backend;

		public LabelSqliteBackendTest()
		{
			var conv = new DbReaderToLabelConverter();
			var settings = new BackendSettingsTestService();
			_path = settings.GetSqliteBackendPath();

			_backend = new LabelSqliteBackend(settings, conv, new DbReaderToLabelsConverter(conv));
		}

		[Fact(DisplayName = "GetAllLabels EmptyDb Returns EmptyList")]
		public async void GetAllLabelsAsync_EmptyDb_Returns_EmptyList()
		{
			var labels = await _backend.GetAllLabelsAsync();

			labels.Should().BeEmpty();
		}

		[Fact]
		public async void InsertLabelAsync_GetLabelAsync_Test()
		{
			var label = FakeDataGenerator.GetNewLabel();

			await _backend.InsertLabelAsync(label);

			var actual = await _backend.GetLabelAsync(label.Id);

			actual.Should().BeEquivalentTo(label);
		}

		public void Dispose()
		{
			File.Delete(Path.GetFullPath(_path));
		}
	}
}