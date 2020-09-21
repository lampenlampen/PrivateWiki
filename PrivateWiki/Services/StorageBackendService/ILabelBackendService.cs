using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.StorageBackendService
{
	public interface ILabelBackendService
	{
		Task<bool> InsertLabelAsync(Label label);

		Task<Label> GetLabelAsync(Guid id);

		Task<IEnumerable<Label>> GetAllLabelsAsync();

		Task<bool> DeleteLabelAsync(Guid id);

		Task<bool> DeleteLabelAsync(Label label);
	}
}