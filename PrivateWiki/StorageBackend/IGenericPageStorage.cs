using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrivateWiki.Models.Pages;

namespace PrivateWiki.StorageBackend
{
	public interface IGenericPageStorage : IPageStorage
	{
		/// <summary>
		/// Returns a Page with the given <paramref name="id"/>
		/// </summary>
		/// <param name="id">The page id</param>
		/// <returns></returns>
		Task<GenericPage> GetPageAsync(Guid id);

		/// <summary>
		/// Returns a page with the given <paramref name="link"/>
		/// </summary>
		/// <param name="link">The page wikilink</param>
		/// <returns></returns>
		Task<GenericPage> GetPageAsync(string link);

		/// <summary>
		/// Returns all pages stored in the <see cref="IStorageBackend"/>.
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<GenericPage>> GetAllPagesAsync();

		/// <summary>
		/// Updates a page in the <see cref="IStorageBackend"/>.
		/// </summary>
		/// <param name="page"></param>
		/// <param name="action"></param>
		/// <returns></returns>
		Task<bool> UpdatePage(GenericPage page, PageAction action);

		/// <summary>
		/// Deletes a page.
		/// </summary>
		/// <returns>True if successful.</returns>
		Task<bool> DeletePageAsync(GenericPage page);

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
		Task<bool> InsertPageAsync(GenericPage page);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> ContainsPageAsync(GenericPage page);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> ContainsPageAsync(Guid id);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="link"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> ContainsPageAsync(string link);

		/// <summary>
		/// Returns the history of a Html page.
		/// </summary>
		/// <param name="pageLink"></param>
		/// <returns></returns>
		Task<IEnumerable<GenericPageHistory>> GetPageHistoryAsync(string pageLink);
	}
}