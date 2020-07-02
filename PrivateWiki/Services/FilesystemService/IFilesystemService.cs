using System.Collections.Generic;
using System.Threading.Tasks;
using PrivateWiki.DataModels;

namespace PrivateWiki.Services.FilesystemService
{
	public interface IFilesystemService
	{
		Task WriteTextAsync(File file, string content);

		Task<File> PickFile(string fileExtension);

		Task<string> ReadTextAsync(File file);

		Task<Folder> GetDataFolder();

		Task<IEnumerable<Folder>> GetAllFolders(Folder folder);
	}
}