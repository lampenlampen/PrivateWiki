using NodaTime;

namespace Models.Pages
{
	public class PageHistory<T> where T : Page
	{
		public T Page { get; private set; }

		public Instant ValidFrom { get; set; }

		public Instant ValidTo { get; set; }

		public PageAction Action { get; set; }

		public PageHistory(T page)
		{
			Page = page;
		}

		public PageHistory(T page, Instant validFrom, Instant validTo)
		{
			Page = page;
			ValidFrom = validFrom;
			ValidTo = validTo;
		}
	}

	public class MarkdownPageHistory : PageHistory<MarkdownPage>
	{
		public MarkdownPageHistory(MarkdownPage page) : base(page)
		{
		}

		public MarkdownPageHistory(MarkdownPage page, Instant validFrom, Instant validTo) : base(page, validFrom, validTo)
		{
		}
	}

	public class GenericPageHistory : PageHistory<GenericPage>
	{
		public GenericPageHistory(GenericPage page) : base(page)
		{
		}

		public GenericPageHistory(GenericPage page, Instant validFrom, Instant validTo) : base(page, validFrom, validTo)
		{
		}
	}
}