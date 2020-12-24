namespace PrivateWiki.DataModels.Pages
{
	public record Tag
	{
		public TagId Id { get; set; }

		public string Title { get; set; }
	}
}