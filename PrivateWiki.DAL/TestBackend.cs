using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentResults;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.Backends;

namespace PrivateWiki.DAL
{
	public class TestBackend : ILabelBackend, IPageLabelsBackend
	{
		public Task InsertLabelAsync(Label label)
		{
			throw new NotImplementedException();
		}

		public Task<Result<Label>> GetLabelAsync(Guid id) => Task.FromResult(Result.Ok(testData.Single(x => x.Id.Equals(id))));

		public Task<IEnumerable<Label>> GetAllLabelsAsync() => Task.FromResult((IEnumerable<Label>) testData);

		public Task DeleteLabelAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task UpdateLabelAsync(Label label)
		{
			throw new NotImplementedException();
		}

		private static IList<Label> testData = new List<Label>
		{
			new Label("testKey1", "Description 1", System.Drawing.Color.Red.ToColor()),
			new Label("testKey2::testValue2", "Description 2", new Color(0, 0, 0)),
			new Label("testKey3::testValue3", "Description 3", System.Drawing.Color.Indigo.ToColor()),
			new Label("testKey4::testValue4", "Description 4", System.Drawing.Color.BurlyWood.ToColor()),
			new Label("testKey5", "Description 1", System.Drawing.Color.SlateGray.ToColor()),
			new Label("testKey6::testValue6", "Description 6", new Color(0, 0, 0)),
			new Label("testKey7::testValue7", "Description 7", System.Drawing.Color.Blue.ToColor()),
			new Label("testKey8::testValue8", "Description 8", System.Drawing.Color.Orange.ToColor()),
			new Label("testKey9", "Description 1", System.Drawing.Color.DodgerBlue.ToColor()),
			new Label("testKey10::testValue10", "Description 10", new Color(0, 0, 0)),
			new Label("testKey11::testValue11", "Description 11", System.Drawing.Color.Green.ToColor()),
			new Label("testKey12::testValue12", "Description 12", System.Drawing.Color.Indigo.ToColor())
		};

		private static IList<Guid> pageLabels = new List<Guid>
		{
			testData[0].Id,
			testData[1].Id
		};

		public Task<IEnumerable<Guid>> GetLabelIdsForPageId(Guid pageId) => Task.FromResult((IEnumerable<Guid>) pageLabels);

		public Task AddLabelToPage(Guid pageId, Guid labelId)
		{
			pageLabels.Add(labelId);

			return Task.CompletedTask;
		}

		public Task DeleteLabel(Guid labelId)
		{
			throw new NotImplementedException();
		}

		public Task DeletePage(Guid pageId)
		{
			throw new NotImplementedException();
		}

		public Task RemoveLabelFromPage(Guid pageId, Guid labelId)
		{
			throw new NotImplementedException();
		}
	}
}