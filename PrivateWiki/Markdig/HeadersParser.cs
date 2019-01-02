using System.IO;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Markdig.Renderers;
using Markdig.Syntax;

namespace PrivateWiki.Markdig
{
	internal class HeadersParser
	{
		private TreeViewNode headerLevel1;
		private TreeViewNode headerLevel2;
		private TreeViewNode headerLevel3;
		private TreeViewNode headerLevel4;
		private TreeViewNode headerLevel5;
		private TreeViewNode headerLevel6;

		private readonly HtmlRenderer renderer = new HtmlRenderer(new StringWriter());
		private TreeViewNode rootHeader;


		public TreeViewNode ParseHeaders(MarkdownDocument doc)
		{
			rootHeader = new TreeViewNode {Content = "test", IsExpanded = true};

			var headerBlocks = doc.Where(b => b is HeadingBlock);

			foreach (HeadingBlock header in headerBlocks)
			{
				renderer.Writer = new StringWriter();

				renderer.WriteLeafInline(header);
				renderer.Writer.Flush();
				var text = renderer.Writer.ToString();

				switch (header.Level)
				{
					case 1:
						AddHeaderLevel1(text);
						break;
					case 2:
						AddHeaderLevel2(text);
						break;
					case 3:
						AddHeaderLevel3(text);
						break;
					case 4:
						AddHeaderLevel4(text);
						break;
					case 5:
						AddHeaderLevel5(text);
						break;
					case 6:
						AddHeaderLevel6(text);
						break;
				}
			}

			return rootHeader;
		}

		private void AddHeaderLevel1(string text)
		{
			AddHeaderLevel1(new TreeViewNode {Content = text});
		}

		private void AddHeaderLevel1(TreeViewNode header)
		{
			rootHeader.Children.Add(header);
			headerLevel1 = header;
			headerLevel2 = null;
			headerLevel3 = null;
			headerLevel4 = null;
			headerLevel5 = null;
			headerLevel6 = null;
		}

		private void AddHeaderLevel2(string text)
		{
			AddHeaderLevel2(new TreeViewNode {Content = text});
		}

		private void AddHeaderLevel2(TreeViewNode header)
		{
			if (headerLevel1 == null) AddHeaderLevel1(new TreeViewNode());

			headerLevel1.Children.Add(header);
			headerLevel2 = header;
			headerLevel3 = null;
			headerLevel4 = null;
			headerLevel5 = null;
			headerLevel6 = null;
		}

		private void AddHeaderLevel3(string text)
		{
			AddHeaderLevel3(new TreeViewNode {Content = text});
		}

		private void AddHeaderLevel3(TreeViewNode header)
		{
			if (headerLevel2 == null) AddHeaderLevel2(new TreeViewNode());

			headerLevel2.Children.Add(header);
			headerLevel3 = header;
			headerLevel4 = null;
			headerLevel5 = null;
			headerLevel6 = null;
		}

		private void AddHeaderLevel4(string text)
		{
			AddHeaderLevel4(new TreeViewNode {Content = text});
		}

		private void AddHeaderLevel4(TreeViewNode header)
		{
			if (headerLevel3 == null) AddHeaderLevel3(new TreeViewNode());

			headerLevel3.Children.Add(header);
			headerLevel4 = header;
			headerLevel5 = null;
			headerLevel6 = null;
		}

		private void AddHeaderLevel5(string text)
		{
			AddHeaderLevel5(new TreeViewNode {Content = text});
		}

		private void AddHeaderLevel5(TreeViewNode header)
		{
			if (headerLevel4 == null) AddHeaderLevel4(new TreeViewNode());

			headerLevel4.Children.Add(header);
			headerLevel5 = header;
			headerLevel6 = null;
		}

		private void AddHeaderLevel6(string text)
		{
			AddHeaderLevel6(new TreeViewNode {Content = text});
		}

		private void AddHeaderLevel6(TreeViewNode header)
		{
			if (headerLevel5 == null) AddHeaderLevel5(new TreeViewNode());

			headerLevel5.Children.Add(header);
			headerLevel6 = header;
		}
	}
}