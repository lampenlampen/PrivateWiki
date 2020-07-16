using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using FluentResults;
using Microsoft.Toolkit.Diagnostics;
using PrivateWiki.DataModels;
using PrivateWiki.Services.ApplicationDataService;
using File = PrivateWiki.DataModels.File;

namespace PrivateWiki.UWP.Services.FilesystemService
{
	public class ApplicationDataService : IApplicationDataService
	{
		public async Task WriteTextAsync(File file, string content)
		{
			var result = await file.ToNative().ConfigureAwait(false);

			if (result.IsFailed)
			{
				// TODO
				throw new Exception("Exception");
			}

			var nativeFile = result.Value;

			await FileIO.WriteTextAsync(nativeFile, content).AsTask().ConfigureAwait(false);
		}

		public async Task<string> ReadTextAsync(File file)
		{
			var result = await file.ToNative().ConfigureAwait(false);

			if (result.IsFailed)
			{
				// TODO 
				throw new Exception("Exception");
			}

			var nativeFile = result.Value;

			return await FileIO.ReadTextAsync(nativeFile).AsTask().ConfigureAwait(false);
		}

		public async Task<Folder> GetDataFolderAsync()
		{
			// TODO hardcoded data-Folder location
			return (await ApplicationData.Current.LocalFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists)).ToFolder();
		}

		public async Task<IEnumerable<Folder>> GetAllFolders(Folder folder)
		{
			throw new System.NotImplementedException();
		}

		private Task<StorageFile> ToNative(File file)
		{
			var storageFile = StorageFile.GetFileFromPathAsync(file.Path).AsTask();
			return storageFile;
		}

		private Task<StorageFolder> ToNative(Folder folder)
		{
			var storageFolder = StorageFolder.GetFolderFromPathAsync(folder.Path).AsTask();
			return storageFolder;
		}
	}

	public static class UWPFileExtensions
	{
		public static async Task<Result<StorageFile>> ToNative(this File file)
		{
			Guard.IsNotNull(file, nameof(file));

			StorageFile nativeFile;

			try
			{
				nativeFile = await StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);
			}
			catch (FileNotFoundException e)
			{
				// TODO include error information
				return Result.Fail(new FileNotFoundError().CausedBy(e));
			}
			catch (UnauthorizedAccessException e)
			{
				// TODO include error information
				return Result.Fail(new UnauthorizedAccessError().CausedBy(e));
			}
			catch (ArgumentException e)
			{
				// TODO include error information
				return Result.Fail(new ArgumentError().CausedBy(e));
			}

			return Result.Ok<StorageFile>(nativeFile);
		}
	}

	public static class UWPFolderExtension
	{
		public static async Task<Result<StorageFolder>> ToNativeAsync(this Folder folder)
		{
			Guard.IsNotNull(folder, nameof(folder));

			StorageFolder nativeFolder;

			try
			{
				nativeFolder = await StorageFolder.GetFolderFromPathAsync(folder.Path).AsTask().ConfigureAwait(false);
			}
			catch (FileNotFoundException e)
			{
				// TODO include error information
				return Result.Fail(new FileNotFoundError().CausedBy(e));
			}
			catch (UnauthorizedAccessException e)
			{
				// TODO include error information
				return Result.Fail(new UnauthorizedAccessError().CausedBy(e));
			}
			catch (ArgumentException e)
			{
				// TODO include error information
				return Result.Fail(new ArgumentError().CausedBy(e));
			}

			return Result.Ok<StorageFolder>(nativeFolder);
		}

		public static Folder ToFolder(this StorageFolder nativeFolder)
		{
			Guard.IsNotNull(nativeFolder, nameof(nativeFolder));

			var folder = new Folder(nativeFolder.Path, nativeFolder.Name);

			return folder;
		}

		public static async Task<bool> HasSubfolders(this StorageFolder folder)
		{
			Guard.IsNotNull(folder, nameof(folder));

			var folders = await folder.GetFoldersAsync();

			return folders.Count > 0;
		}
	}
}