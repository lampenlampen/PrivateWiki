using System;
using FluentAssertions;
using FluentAssertions.Execution;
using PrivateWiki.Services.Backends;
using PrivateWiki.Services.Backends.Sqlite;
using PrivateWiki.Test.Services.StorageBackendService.SQLite;
using Xunit;

namespace PrivateWiki.Test.Services.Backends
{
	public class PageLabelsSqliteBackendTest
	{
		private readonly IPageLabelsBackend _backend;

		public PageLabelsSqliteBackendTest()
		{
			var settings = new BackendSettingsTestService();

			_backend = new PageLabelsSqliteBackend(settings, new DbReaderToLabelIdsConverter());
		}

		[Fact(DisplayName = "GetLabelsForPageId EmptyTable returns empty list")]
		public async void GetLabelsForPageId_EmptyTable_Returns_EmptyList()
		{
			var actual = await _backend.GetLabelIdsForPageId(Guid.NewGuid());

			actual.Should().BeEmpty();
		}

		[Fact(DisplayName = "GetLabelsForPageId UnknownPageId Returns EmptyList")]
		public async void GetLabelsForPageId_UnknownPageId_Returns_EmptyList()
		{
			await _backend.AddLabelToPage(Guid.NewGuid(), Guid.NewGuid());
			await _backend.AddLabelToPage(Guid.NewGuid(), Guid.NewGuid());
			await _backend.AddLabelToPage(Guid.NewGuid(), Guid.NewGuid());

			var actual = await _backend.GetLabelIdsForPageId(Guid.NewGuid());

			actual.Should().BeEmpty();
		}

		[Fact]
		public async void AddLabelToPageTest()
		{
			var labelId = Guid.NewGuid();
			var pageId = Guid.NewGuid();

			await _backend.AddLabelToPage(pageId, labelId);

			var actualLabelIds = await _backend.GetLabelIdsForPageId(pageId);

			using (new AssertionScope())
			{
				actualLabelIds.Should().HaveCount(1);
				actualLabelIds.Should().Contain(labelId);
			}
		}

		[Fact]
		public async void DeleteLabelTest()
		{
			var labelId = Guid.NewGuid();
			var pageId1 = Guid.NewGuid();
			var pageId2 = Guid.NewGuid();

			await _backend.AddLabelToPage(pageId1, labelId);
			await _backend.AddLabelToPage(pageId2, labelId);
			await _backend.AddLabelToPage(pageId1, Guid.NewGuid());
			await _backend.AddLabelToPage(pageId2, Guid.NewGuid());

			await _backend.DeleteLabel(labelId);

			var labels1 = await _backend.GetLabelIdsForPageId(pageId1);
			var labels2 = await _backend.GetLabelIdsForPageId(pageId2);

			using (new AssertionScope())
			{
				labels1.Should().NotContain(labelId);
				labels2.Should().NotContain(labelId);
			}
		}

		[Fact]
		public async void DeletePageTest()
		{
			var labelId1 = Guid.NewGuid();
			var labelId2 = Guid.NewGuid();
			var pageId1 = Guid.NewGuid();
			var pageId2 = Guid.NewGuid();

			await _backend.AddLabelToPage(pageId1, labelId1);
			await _backend.AddLabelToPage(pageId1, labelId2);
			await _backend.AddLabelToPage(pageId2, labelId1);
			await _backend.AddLabelToPage(pageId2, labelId2);
			await _backend.AddLabelToPage(pageId1, Guid.NewGuid());
			await _backend.AddLabelToPage(pageId2, Guid.NewGuid());

			await _backend.DeletePage(pageId1);

			var labels1 = await _backend.GetLabelIdsForPageId(pageId1);
			var labels2 = await _backend.GetLabelIdsForPageId(pageId2);

			using (new AssertionScope())
			{
				labels1.Should().BeEmpty();
				labels2.Should().Contain(labelId1);
				labels2.Should().Contain(labelId2);
				labels2.Should().HaveCount(3);
			}
		}

		[Fact]
		public async void RemoveLabelFromPageTest()
		{
			var labelId1 = Guid.NewGuid();
			var pageId1 = Guid.NewGuid();

			await _backend.AddLabelToPage(pageId1, labelId1);

			await _backend.RemoveLabelFromPage(pageId1, labelId1);

			var labels = await _backend.GetLabelIdsForPageId(pageId1);

			labels.Should().BeEmpty();
		}
	}
}