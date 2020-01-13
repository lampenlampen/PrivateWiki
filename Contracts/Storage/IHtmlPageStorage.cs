using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Pages;

namespace Contracts.Storage
{
	public interface IHtmlPageStorage : IPageStorage
	{
		Task<HtmlPage> GetHtmlPageAsync(Guid id);

		Task<HtmlPage> GetMarkdownPageAsync(string link);

		Task<IEnumerable<MarkdownPage>> GetAllMarkdownPagesAsync();
	}
}