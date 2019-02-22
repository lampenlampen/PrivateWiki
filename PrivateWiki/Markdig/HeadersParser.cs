using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Markdig.Renderers;
using Markdig.Renderers.Html;
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

		private Hashtable HeaderIDs { get; } = new Hashtable();


		public Tuple<IList<TreeViewNode>, Hashtable> ParseHeaders(MarkdownDocument doc)
		{
			rootHeader = new TreeViewNode {Content = "test", IsExpanded = true};

			var headerBlocks = doc.Where(b => b is HeadingBlock);

			foreach (HeadingBlock header in headerBlocks)
			{
				var attributes = header.TryGetAttributes();
				var id = attributes.Id;
				renderer.Writer = new StringWriter();

				renderer.WriteLeafInline(header);
				renderer.Writer.Flush();
				var text = renderer.Writer.ToString();

				switch (header.Level)
				{
					case 1:
						AddHeaderLevel1(text, id);
						break;
					case 2:
						AddHeaderLevel2(text, id);
						break;
					case 3:
						AddHeaderLevel3(text, id);
						break;
					case 4:
						AddHeaderLevel4(text, id);
						break;
					case 5:
						AddHeaderLevel5(text, id);
						break;
					case 6:
						AddHeaderLevel6(text, id);
						break;
				}
			}

			return Tuple.Create(rootHeader.Children, HeaderIDs);
		}

		private void AddHeaderLevel1(string text, string id)
		{
			HeaderIDs.Add(text, id);

			AddHeaderLevel1(new TreeViewNode
			{
				Content = text,
			});
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

		private void AddHeaderLevel2(string text, string id)
		{
			HeaderIDs.Add(text, id);
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

		private void AddHeaderLevel3(string text, string id)
		{
			HeaderIDs.Add(text, id);
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

		private void AddHeaderLevel4(string text, string id)
		{
			HeaderIDs.Add(text, id);
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

		private void AddHeaderLevel5(string text, string id)
		{
			HeaderIDs.Add(text, id);
			AddHeaderLevel5(new TreeViewNode {Content = text});
		}

		private void AddHeaderLevel5(TreeViewNode header)
		{
			if (headerLevel4 == null) AddHeaderLevel4(new TreeViewNode());

			headerLevel4.Children.Add(header);
			headerLevel5 = header;
			headerLevel6 = null;
		}

		private void AddHeaderLevel6(string text, string id)
		{
			HeaderIDs.Add(text, id);
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