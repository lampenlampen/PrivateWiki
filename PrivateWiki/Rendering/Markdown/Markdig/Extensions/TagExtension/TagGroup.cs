using System.Collections.Generic;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.TagExtension
{
	/// <summary>
	/// A block that contains all the tags at the end of a <see cref="MarkdownDocument"/>.
	/// </summary>
	/// <seealso cref="Markdig.Syntax.ContainerBlock" />
	public class TagGroup : List<TagInline>
	{
	}
}