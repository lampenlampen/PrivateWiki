using System.Threading.Tasks;
using PrivateWiki.DataModels;

namespace PrivateWiki.Services.ApplicationDataService
{
	public interface IApplicationDataService
	{
		public Task WriteTextAsync(File file, string content);

		public Task<string> ReadTextAsync(File file);

		public Task<Folder> GetDataFolderAsync();
	}
}