using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Pages;

namespace Contracts.Storage
{
	/// <summary>
	/// Provides access to a storage backend.
	/// </summary>
	public interface IStorageBackend
	{
		/// <summary>
		/// Checks if a <see cref="IStorageBackend"/> does exist.
		/// </summary>
		/// <returns></returns>
		Task<bool> ExistsAsync();

		/// <summary>
		/// Returns a Page with the given <paramref name="id"/>
		/// </summary>
		/// <param name="id">The page id</param>
		/// <returns></returns>
		Task<Page> GetPageAsync(Guid id);

		/// <summary>
		/// Returns a page with the given <paramref name="link"/>
		/// </summary>
		/// <param name="link">The page wikilink</param>
		/// <returns></returns>
		Task<Page> GetPageAsync(string link);

		/// <summary>
		/// Returns all pages stored in the <see cref="IStorageBackend"/>.
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<Page>> GetAllPagesAsync();

		/// <summary>
		/// Updates a page in the <see cref="IStorageBackend"/>.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> UpdatePage(Page page);

		/// <summary>
		/// Deletes a page.
		/// </summary>
		/// <returns>True if successful.</returns>
		Task<bool> DeletePageAsync(Page page);

		/// <summary>
		/// Deletes the page with the <paramref name="id"/>
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> DeletePageAsync(Guid id);

		/// <summary>
		/// Inserts a new page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> InsertPageAsync(Page page);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		bool ContainsPageAsync(Page page);
		
		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		bool ContainsPageAsync(Guid id);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		bool ContainsPageAsync(string link);

		/// <summary>
		/// Deletes the <see cref="IStorageBackend"/>
		/// </summary>
		/// <returns></returns>
		bool Delete();
	}
}