using System.Threading.Tasks;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.PackageService;
using PrivateWiki.Services.StorageBackendService;

namespace PrivateWiki.Services.StartupTask
{
	public class InsertDefaultPagesStartupTask : IStartupTask
	{
		private readonly IPageBackendService _backend;

		private readonly IAssetsService _assetsService;

		private readonly IClock _clock;

		public InsertDefaultPagesStartupTask(IAssetsService assetsService, IPageBackendService backend, IClock clock)
		{
			_backend = backend;
			_assetsService = assetsService;
			_clock = clock;
		}

		public Task<bool> Execute() => InsertDefaultPages();

		private async Task<bool> InsertStartPage()
		{
			if (await _backend.ContainsPageAsync("start").ConfigureAwait(false)) return false;

			var content = await _assetsService.GetStartPage().ConfigureAwait(false);

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("start"), content, ContentType.Markdown, instant, instant, false);

			await _backend.InsertPageAsync(startPage).ConfigureAwait(false);

			return true;
		}

		private async Task<bool> InsertSyntaxPage()
		{
			if (await _backend.ContainsPageAsync("system:syntax").ConfigureAwait(false)) return false;

			var content = await _assetsService.GetSyntaxPage().ConfigureAwait(false);

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:syntax"), content, ContentType.Markdown, instant, instant, true);

			await _backend.InsertPageAsync(startPage).ConfigureAwait(false);

			return true;
		}

		private async Task<bool> InsertMarkdownTestPage()
		{
			if (await _backend.ContainsPageAsync("system:markdowntest").ConfigureAwait(false)) return false;

			var content = await _assetsService.GetMarkdownTestPage().ConfigureAwait(false);

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:markdowntest"), content, ContentType.Markdown, instant, instant, true);

			await _backend.InsertPageAsync(startPage).ConfigureAwait(false);

			return true;
		}

		private async Task<bool> InsertHtmlTestPage()
		{
			if (await _backend.ContainsPageAsync("system:htmltest").ConfigureAwait(false)) return false;

			var content = await _assetsService.GetHtmlTestPage().ConfigureAwait(false);

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:htmltest"), content, ContentType.Html, instant, instant, true);

			await _backend.InsertPageAsync(startPage).ConfigureAwait(false);

			return true;
		}

		private async Task<bool> InsertTextTestPage()
		{
			if (await _backend.ContainsPageAsync("system:texttest").ConfigureAwait(false)) return false;

			var content = await _assetsService.GetTextTestPage().ConfigureAwait(false);

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:texttest"), content, ContentType.Text, instant, instant, true);

			await _backend.InsertPageAsync(startPage).ConfigureAwait(false);

			return true;
		}

		private async Task<bool> InsertDefaultPages()
		{
			var start = await InsertStartPage().ConfigureAwait(false);
			var syntax = await InsertSyntaxPage().ConfigureAwait(false);
			var markdown = await InsertMarkdownTestPage().ConfigureAwait(false);
			var html = await InsertHtmlTestPage().ConfigureAwait(false);
			var text = await InsertTextTestPage().ConfigureAwait(false);

			return start && syntax && markdown && html && text;
		}
	}
}