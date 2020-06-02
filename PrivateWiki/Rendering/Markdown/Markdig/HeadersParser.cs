namespace PrivateWiki.Rendering.Markdown.Markdig
{
	public class HeadersParser
	{
		/*
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

			if (rootHeader.Children.Count > 1)
			{
				// TODO Refactor
				throw new Exception("Only 1 Top Level Heading allowed.");
			}

			return rootHeader.Children[0];
		}

		private void AddHeaderLevel1(string text, string id)
		{
			AddHeaderLevel1(new MyTreeViewNode
			{
				Content = text,
				Tag = id
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
			AddHeaderLevel2(new MyTreeViewNode
			{
				Content = text,
				Tag = id
			});
		}

		private void AddHeaderLevel2(TreeViewNode header)
		{
			if (headerLevel1 == null) AddHeaderLevel1(new MyTreeViewNode());

			headerLevel1.Children.Add(header);
			headerLevel2 = header;
			headerLevel3 = null;
			headerLevel4 = null;
			headerLevel5 = null;
			headerLevel6 = null;
		}

		private void AddHeaderLevel3(string text, string id)
		{
			AddHeaderLevel3(new MyTreeViewNode
			{
				Content = text,
				Tag = id
			});
		}

		private void AddHeaderLevel3(TreeViewNode header)
		{
			if (headerLevel2 == null) AddHeaderLevel2(new MyTreeViewNode());

			headerLevel2.Children.Add(header);
			headerLevel3 = header;
			headerLevel4 = null;
			headerLevel5 = null;
			headerLevel6 = null;
		}

		private void AddHeaderLevel4(string text, string id)
		{
			AddHeaderLevel4(new MyTreeViewNode
			{
				Content = text,
				Tag = id
			});
		}

		private void AddHeaderLevel4(TreeViewNode header)
		{
			if (headerLevel3 == null) AddHeaderLevel3(new MyTreeViewNode());

			headerLevel3.Children.Add(header);
			headerLevel4 = header;
			headerLevel5 = null;
			headerLevel6 = null;
		}

		private void AddHeaderLevel5(string text, string id)
		{
			AddHeaderLevel5(new MyTreeViewNode
			{
				Content = text,
				Tag = id
			});
		}

		private void AddHeaderLevel5(TreeViewNode header)
		{
			if (headerLevel4 == null) AddHeaderLevel4(new MyTreeViewNode());

			headerLevel4.Children.Add(header);
			headerLevel5 = header;
			headerLevel6 = null;
		}

		private void AddHeaderLevel6(string text, string id)
		{
			AddHeaderLevel6(new MyTreeViewNode
			{
				Content = text,
				Tag = id
			});
		}

		private void AddHeaderLevel6(TreeViewNode header)
		{
			if (headerLevel5 == null) AddHeaderLevel5(new MyTreeViewNode());

			headerLevel5.Children.Add(header);
			headerLevel6 = header;
		}
		*/
	}
}