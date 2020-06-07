using System.Threading.Tasks;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.PackageService;
using PrivateWiki.Services.StorageBackendService;

namespace PrivateWiki.Services.DefaultPagesService
{
	public class DefaultPagesService : IDefaultPagesService
	{
		private readonly IPageBackendService _backend;

		private readonly IAssetsService _assetsService;

		private readonly IClock _clock;

		public DefaultPagesService(IAssetsService assetsService, IPageBackendService backend, IClock clock)
		{
			_backend = backend;
			_assetsService = assetsService;
			_clock = clock;
		}

		public async Task<bool> InsertStartPage()
		{
			if (await _backend.ContainsPageAsync("start")) return false;

			var content = await _assetsService.GetStartPage();

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("start"), content, ContentType.Markdown, instant, instant, false);

			await _backend.InsertPageAsync(startPage);

			return true;
		}

		public async Task<bool> InsertSyntaxPage()
		{
			if (await _backend.ContainsPageAsync("system:syntax")) return false;

			var content = await _assetsService.GetSyntaxPage();

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:syntax"), content, ContentType.Markdown, instant, instant, false);

			await _backend.InsertPageAsync(startPage);

			return true;
		}

		public async Task<bool> InsertMarkdownTestPage()
		{
			if (await _backend.ContainsPageAsync("system:markdowntest")) return false;

			var content = await _assetsService.GetMarkdownTestPage();

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:markdowntest"), content, ContentType.Markdown, instant, instant, false);

			await _backend.InsertPageAsync(startPage);

			return true;
		}

		public async Task<bool> InsertHtmlTestPage()
		{
			if (await _backend.ContainsPageAsync("system:htmltest")) return false;

			var content = await _assetsService.GetHtmlTestPage();

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:htmltest"), content, ContentType.Markdown, instant, instant, false);

			await _backend.InsertPageAsync(startPage);

			return true;
		}

		public async Task<bool> InsertTextTestPage()
		{
			if (await _backend.ContainsPageAsync("system:texttest")) return false;

			var content = await _assetsService.GetTextTestPage();

			var instant = _clock.GetCurrentInstant();

			var startPage = new GenericPage(Path.ofLink("system:texttest"), content, ContentType.Markdown, instant, instant, false);

			await _backend.InsertPageAsync(startPage);

			return true;
		}

		public async Task<bool> InsertDefaultPages()
		{
			var start = await InsertStartPage();
			var syntax = await InsertSyntaxPage();
			var markdown = await InsertMarkdownTestPage();
			var html = await InsertHtmlTestPage();
			var text = await InsertTextTestPage();

			return start && syntax && markdown && html && text;
		}
	}
}