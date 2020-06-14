using System.Threading.Tasks;
using PrivateWiki.DataModels;

namespace PrivateWiki.Services.FileExplorerService
{
	public interface IFileExplorerService
	{
		Task<bool> ShowFolderAsync(Folder folder);
	}
}