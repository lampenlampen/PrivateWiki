using System;
using System.IO;

namespace PrivateWiki.Rendering
{
	public class HtmlBuilder : IDisposable
	{
		private TextWriter _htmlWriter;

		public HtmlBuilder()
		{
			_htmlWriter = new StringWriter();
		}

		public HtmlBuilder(TextWriter headWriter)
		{
			_htmlWriter = headWriter;
		}

		public void WriteHtmlStartTag()
		{
			_htmlWriter.WriteLine("<html>");
		}

		public void WriteHtmlEndTag()
		{
			_htmlWriter.WriteLine("</html>");
		}

		public void WriteHeadStartTag()
		{
			_htmlWriter.WriteLine("<head>");
		}

		public void WriteHeadEndTag()
		{
			_htmlWriter.WriteLine("</head>");
		}

		public void WriteBodyStartTag()
		{
			_htmlWriter.WriteLine("<body>");
		}

		public void WriteBodyEndTag()
		{
			_htmlWriter.WriteLine("</body>");
		}

		public void WriteScriptTag(string script)
		{
			_htmlWriter.Write("<script>");
			_htmlWriter.Write(script);
			_htmlWriter.WriteLine("</script>");
		}

		public void LoadScriptFromUrl(Uri url)
		{
			_htmlWriter.WriteLine($"<script type=\"text/javascript\" src=\"{url.AbsoluteUri}\"></script>");
		}

		public void LoadScriptAsyncFromUrl(Uri url)
		{
			_htmlWriter.WriteLine($"<script type=\"text/javascript\" async src=\"{url.AbsoluteUri}\"></script>");
		}

		public void LoadStylesheetFromUrl(Uri url)
		{
			_htmlWriter.WriteLine($"<link rel=\"stylesheet\" type=\"text/css\" href=\"{url.AbsoluteUri}\" </link>");
		}

		public void WriteHtmlSnippet(string element)
		{
			_htmlWriter.WriteLine(element);
		}

		public void Close()
		{
			_htmlWriter.Close();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_htmlWriter?.Dispose();
				_htmlWriter = null!;
			}
		}

		public void Flush()
		{
			_htmlWriter.Flush();
		}

		public override string ToString()
		{
			_htmlWriter.Write("</head");
			_htmlWriter.Flush();
			return _htmlWriter.ToString();
		}
	}
}