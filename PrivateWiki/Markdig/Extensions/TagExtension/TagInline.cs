using Markdig.Syntax.Inlines;

namespace PrivateWiki.Markdig.Extensions.TagExtension
{
	/// <summary>
	/// Represents a tag.
	/// </summary>
	public class TagInline : LeafInline
	{
		public string Content { get; set; }
	}
}