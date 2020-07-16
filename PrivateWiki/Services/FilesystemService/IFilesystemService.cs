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

		Task<Result<IEnumerable<Folder>>> GetAllFoldersAsync(Folder folder);

		Task<Result<Folder>> CreateFolderAsync(Folder folder, string folderName);

		/// <summary>
		/// Creates a new file in the given folder.
		/// </summary>
		/// <param name="folder"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		Task<Result<File>> CreateFileAsync(Folder folder, string fileName);

		Task<Result<Folder>> CopyAsync(Folder source, Folder target);

		Task<Result<File>> CopyAsync(File source, Folder target);
	}
}