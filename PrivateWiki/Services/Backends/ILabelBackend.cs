using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.Backends
{
	public interface ILabelBackend
	{
		Task InsertLabelAsync(Label label);

		Task<Result<Label>> GetLabelAsync(Guid id);

		Task<IEnumerable<Label>> GetAllLabelsAsync();

		Task DeleteLabelAsync(Guid id);

		Task UpdateLabelAsync(Label label);
	}
}