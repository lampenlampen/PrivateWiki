using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Pages;

namespace Contracts.Storage
{
	public interface IMarkdownPageStorage : IPageStorage
	{
		/// <summary>
		/// Returns a Page with the given <paramref name="id"/>
		/// </summary>
		/// <param name="id">The page id</param>
		/// <returns></returns>
		Task<MarkdownPage> GetMarkdownPageAsync(Guid id);

		/// <summary>
		/// Returns a page with the given <paramref name="link"/>
		/// </summary>
		/// <param name="link">The page wikilink</param>
		/// <returns></returns>
		Task<MarkdownPage> GetMarkdownPageAsync(string link);

		/// <summary>
		/// Returns all pages stored in the <see cref="IStorageBackend"/>.
		/// </summary>
		/// <returns></returns>
		Task<IEnumerable<MarkdownPage>> GetAllMarkdownPagesAsync();

		/// <summary>
		/// Updates a page in the <see cref="IStorageBackend"/>.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> UpdateMarkdownPage(MarkdownPage page);

		/// <summary>
		/// Deletes a page.
		/// </summary>
		/// <returns>True if successful.</returns>
		Task<bool> DeleteMarkdownPageAsync(MarkdownPage page);

		/// <summary>
		/// Deletes the page with the <paramref name="id"/>
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<bool> DeleteMarkdownPageAsync(Guid id);

		/// <summary>
		/// Inserts a new page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> InsertMarkdownPageAsync(MarkdownPage page);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> ContainsMarkdownPageAsync(MarkdownPage page);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> ContainsMarkdownPageAsync(Guid id);

		/// <summary>
		/// Checks if the <see cref="IStorageBackend"/> contains a page.
		/// </summary>
		/// <param name="link"></param>
		/// <param name="page"></param>
		/// <returns></returns>
		Task<bool> ContainsMarkdownPageAsync(string link);

		/// <summary>
		/// Returns the history of a markdown page.
		/// </summary>
		/// <param name="pageLink"></param>
		/// <returns></returns>
		Task<IEnumerable<HistoryMarkdownPage>> GetHistory(string pageLink);
	}
}