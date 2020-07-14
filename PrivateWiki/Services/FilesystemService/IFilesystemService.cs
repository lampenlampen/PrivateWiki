using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults;
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

		Task<Result<Folder>> CreateFolder(Folder folder, string folderName);

		/// <summary>
		/// Creates a new file in the given folder.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		Task<Result<File>> CreateFile(Folder folder, string fileName);

		Task<Folder> Copy(Folder source, Folder target);

		Task<File> Copy(File source, Folder target);
	}
}