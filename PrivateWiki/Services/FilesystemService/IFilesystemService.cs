using System.Collections.Generic;
using System.Threading.Tasks;
using PrivateWiki.DataModels;

namespace PrivateWiki.Services.FilesystemService
{
	public interface IFilesystemService
	{
		Task<File> WriteTextAsync(File file, string content);

		Task<File> PickFile(string fileExtension);

		Task<string> ReadTextAsync(File file);

		Task<Folder> GetDataFolder();

		Task<IEnumerable<Folder>> GetAllFolders(Folder folder);

		Task<Folder> CreateFolder(Folder folder, string folderName);

		Task<File> CreateFile(Folder folder, string fileName);

		Task<Folder> Copy(Folder source, Folder target);

		Task<File> Copy(File source, Folder target);
	}
}