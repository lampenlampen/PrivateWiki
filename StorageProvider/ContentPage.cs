using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NodaTime;

namespace StorageProvider
{
	[Obsolete("Use DataAccessLibrary.PageModel instead.", true)]
	public class ContentPage
	{
		/// <summary>
		///     The unique string id
		/// </summary>
		[Key]
		public string Id { get; set; }

		/// <summary>
		///     The markdown formatted content
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		///     Date of Creation
		/// </summary>
		public Instant CreationTime { get; set; }

		/// <summary>
		///     Date of last Change
		/// </summary>
		public Instant ChangeTime { get; set; }

		/// <summary>
		///     Date of last change (for versioning)
		/// </summary>
		public Instant ValidFrom { get; set; }

		/// <summary>
		///     Date of next change (for versioning)
		/// </summary>
		public Instant ValidTo { get; set; }

		public bool IsFavorite { get; set; }

		public List<Tag> Tags { get; set; } = new List<Tag>();

		/// <summary>
		///     Indicates whether this page can be edited
		/// </summary>
		public bool IsLocked { get; set; }

		public string ExternalFileToken { get; set; }

		public Instant? ExternalFileImportDate { get; set; }

		public ContentPage(string id, string content)
		{
			Id = id;
			Content = content;
		}

		public ContentPage(string id, string content, string token)
		{
			Id = id;
			Content = content;
			ExternalFileToken = token;
		}

		public static ContentPage Create(string id)
		{
			var welcome = $"# {id}";
			return new ContentPage(id, welcome);
		}

		public static ContentPage CreateExternalPage(string id, string token, IClock clock)
		{
			return new ContentPage(id, $"# {id}", token){ ExternalFileImportDate = clock.GetCurrentInstant()};
		}

		public ContentPage Clone()
		{
			var page = new ContentPage(Id, Content)
			{
				ChangeTime = ChangeTime,
				CreationTime = CreationTime,
				IsFavorite = IsFavorite,
				IsLocked = IsLocked,
				Tags = Tags,
				ValidFrom = ValidFrom,
				ValidTo = ValidTo
			};
			return page;
		}
	}
}