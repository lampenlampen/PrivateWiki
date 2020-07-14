using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using ColorCode.Common;
using FluentResults;
using NLog;
using PrivateWiki.DataModels;
using PrivateWiki.Services.FilesystemService;
using File = PrivateWiki.DataModels.File;

namespace PrivateWiki.UWP.Services.FilesystemService
{
	public class UWPFullTrustFilesystemService : IFilesystemService
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public async Task<File> WriteTextAsync(File file, string content)
		{
			var storageFile = await StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);

			await FileIO.WriteTextAsync(storageFile, content);

			return file;
		}

		public async Task<File> PickFile(string fileExtension)
		{
			var picker = new FileOpenPicker
			{
				ViewMode = PickerViewMode.List,
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			picker.FileTypeFilter.Add(fileExtension);

			var storageFile = await picker.PickSingleFileAsync();

			return new File(storageFile.Path);
		}

		public async Task<string> ReadTextAsync(File file)
		{
			var storageFile = await ToNative(file);

			return await FileIO.ReadTextAsync(storageFile);
		}

		private async Task<StorageFile> ToNative(File file)
		{
			var path = file.Path.Replace('\\', '/');

			var storageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(path);
			return storageFile;
		}

		private async Task<Result<StorageFile>> ToNativeAsync(File file)
		{
			StorageFile storageFile;
			try
			{
				storageFile = await StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);
			}
			catch (FileNotFoundException e)
			{
				// The file does not exist
				Logger.Info(e);
				return Result.Fail<StorageFile>(new FileNotFoundError("The file does not exist").CausedBy(e));
			}
			catch (UnauthorizedAccessException e)
			{
				// Insufficient permission to access this folder.
				Logger.Info(e);
				return Result.Fail<StorageFile>(new UnauthorizedAccessError("Insufficient permission to access this file.").CausedBy(e));
			}
			catch (ArgumentException e)
			{
				// The path cannot be a relative path or a Uri.
				Logger.Info(e);
				return Result.Fail<StorageFile>(new ArgumentError("The path cannot be a relative path or a Uri.").CausedBy(e));
			}

			return Result.Ok(storageFile);
		}

		private async Task<Result<StorageFolder>> ToNativeAsync(Folder folder)
		{
			StorageFolder storageFolder;
			try
			{
				storageFolder = await StorageFolder.GetFolderFromPathAsync(folder.Path).AsTask().ConfigureAwait(false);
			}
			catch (FileNotFoundException e)
			{
				// The folder does not exist
				Logger.Info(e);
				return Result.Fail<StorageFolder>(new FileNotFoundError("The folder does not exist").CausedBy(e));
			}
			catch (UnauthorizedAccessException e)
			{
				// Insufficient permission to access this folder.
				Logger.Info(e);
				return Result.Fail<StorageFolder>(new UnauthorizedAccessError("Insufficient permission to access this folder.").CausedBy(e));
			}
			catch (ArgumentException e)
			{
				// The path cannot be a relative path or a Uri.
				Logger.Info(e);
				return Result.Fail<StorageFolder>(new ArgumentError("The path cannot be a relative path or a Uri.").CausedBy(e));
			}

			return Result.Ok(storageFolder);
		}

		public async Task<Folder> GetDataFolder()
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Folder>> GetAllFolders(Folder root)
		{
			throw new NotImplementedException();
		}

		public async Task<Result<Folder>> CreateFolder(Folder folder, string folderName)
		{
			Guard.ArgNotNull(folder, nameof(folder));
			Guard.ArgNotNullAndNotEmpty(folderName, nameof(folderName));

			var result = await ToNativeAsync(folder).ConfigureAwait(false);

			if (result.IsFailed)
			{
				return result.ToResult<Folder>();
			}

			StorageFolder storageFolder = result.Value;
			StorageFolder newStorageFolder;
			try
			{
				newStorageFolder = await storageFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
			}
			catch (FileNotFoundException e)
			{
				// The folder name contains invalid characters or the format of the name is invalid.
				Logger.Info(e);
				return Result.Fail<Folder>(new ArgumentError("The folder name contains invalid characters or the format of the name is invalid.").CausedBy(e));
			}
			catch (UnauthorizedAccessException e)
			{
				// Insufficient permission to access this folder.
				Logger.Info(e);
				return Result.Fail<Folder>(new UnauthorizedAccessError("Insufficient permission to access this folder.").CausedBy(e));
			}

			return Result.Ok(new Folder(newStorageFolder.Path, newStorageFolder.Name));
		}

		public async Task<Result<File>> CreateFile(Folder folder, string fileName)
		{
			Guard.ArgNotNull(folder, nameof(folder));
			Guard.ArgNotNullAndNotEmpty(fileName, nameof(fileName));

			var result = await ToNativeAsync(folder).ConfigureAwait(false);

			if (result.IsFailed)
			{
				return result.ToResult<File>();
			}

			StorageFolder storageFolder = result.Value;

			StorageFile newStorageFile;
			try
			{
				newStorageFile = await storageFolder.CreateFileAsync(fileName.Replace(':', '_'), CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
			}
			catch (FileNotFoundException e)
			{
				// The folder name contains invalid characters or the format of the name is invalid.
				Logger.Info(e);
				return Result.Fail<File>(new ArgumentError("The file name contains invalid characters or the format of the name is invalid.").CausedBy(e));
			}
			catch (UnauthorizedAccessException e)
			{
				// Insufficient permission to create a file in the folder.
				Logger.Info(e);
				return Result.Fail<File>(new UnauthorizedAccessError("Insufficient permission to create a file in the folder.").CausedBy(e));
			}

			return Result.Ok(new File(newStorageFile.Path));
		}

		public async Task<Folder> Copy(Folder source, Folder target)
		{
			throw new NotImplementedException();
		}

		public async Task<File> Copy(File source, Folder target)
		{
			throw new NotImplementedException();
		}
	}
}