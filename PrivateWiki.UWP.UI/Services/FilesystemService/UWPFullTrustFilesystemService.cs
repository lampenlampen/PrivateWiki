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
using PrivateWiki.DataModels.Errors;
using PrivateWiki.Services.FilesystemService;
using File = PrivateWiki.DataModels.File;

namespace PrivateWiki.UWP.UI.Services.FilesystemService
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

		public async Task<Result<IEnumerable<Folder>>> GetAllFoldersAsync(Folder root)
		{
			Guard.ArgNotNull(root, nameof(root));

			var result = await ToNativeAsync(root).ConfigureAwait(false);

			if (result.IsFailed) return result.ToResult<IEnumerable<Folder>>();

			var rootStorageFolder = result.Value;

			var folders = await rootStorageFolder.GetFoldersAsync().AsTask().ConfigureAwait(false);

			IList<Folder> list = new List<Folder>();

			foreach (var storageFolder in folders)
			{
				list.Add(new Folder(storageFolder.Path, storageFolder.Name));
			}

			return Result.Ok<IEnumerable<Folder>>(list);
		}

		public async Task<Result<Folder>> CreateFolderAsync(Folder folder, string folderName)
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

		public async Task<Result<File>> CreateFileAsync(Folder folder, string fileName)
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

		public async Task<Result<Folder>> CopyAsync(Folder source, Folder target)
		{
			Guard.ArgNotNull(source, nameof(source));
			Guard.ArgNotNull(target, nameof(target));

			var targetFolderTask = ToNativeAsync(target);
			var sourceFolderTask = ToNativeAsync(source);

			await Task.WhenAll(targetFolderTask, sourceFolderTask).ConfigureAwait(false);

			var result2 = Result.Merge(await targetFolderTask, await sourceFolderTask);

			if (result2.IsFailed) return result2.ToResult<Folder>();

			var targetContainer = (await targetFolderTask).Value;
			var sourceStorageFolder2 = (await sourceFolderTask).Value;

			var targetStorageFolder = await targetContainer.CreateFolderAsync(sourceStorageFolder2.Name, CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);

			await CopyAsync(sourceStorageFolder2, targetContainer);

			return Result.Ok(new Folder(targetStorageFolder.Path, targetStorageFolder.Name));
		}

		private async Task CopyAsync(StorageFolder source, StorageFolder target)
		{
			StorageFolder destinationFolder = await target.CreateFolderAsync(source.Name, CreationCollisionOption.ReplaceExisting);

			foreach (var file in await source.GetFilesAsync())
			{
				await file.CopyAsync(destinationFolder, file.Name, NameCollisionOption.ReplaceExisting);
			}

			foreach (var folder in await source.GetFoldersAsync())
			{
				await CopyFolderAsync(folder, destinationFolder);
			}
		}

		public static async Task CopyFolderAsync(StorageFolder source, StorageFolder destinationContainer, string desiredName = null)
		{
			StorageFolder destinationFolder = null;
			destinationFolder = await destinationContainer.CreateFolderAsync(
				desiredName ?? source.Name, CreationCollisionOption.ReplaceExisting);

			foreach (var file in await source.GetFilesAsync())
			{
				await file.CopyAsync(destinationFolder, file.Name, NameCollisionOption.ReplaceExisting);
			}

			foreach (var folder in await source.GetFoldersAsync())
			{
				await CopyFolderAsync(folder, destinationFolder);
			}
		}

		public async Task<Result<File>> CopyAsync(File source, Folder target)
		{
			Guard.ArgNotNull(source, nameof(source));
			Guard.ArgNotNull(target, nameof(target));

			var targetFolderTask = ToNativeAsync(target);
			var sourceFileTask = ToNativeAsync(source);

			await Task.WhenAll(targetFolderTask, sourceFileTask).ConfigureAwait(false);

			var result2 = Result.Merge(await targetFolderTask, await sourceFileTask);

			if (result2.IsFailed) return result2.ToResult<File>();

			var targetStorageFolder2 = (await targetFolderTask).Value;
			var sourceStorageFile2 = (await sourceFileTask).Value;

			var targetStorageFile = await sourceStorageFile2.CopyAsync(targetStorageFolder2).AsTask().ConfigureAwait(false);

			return Result.Ok(new File(targetStorageFile.Path));
		}
	}
}