using JetBrains.Annotations;
using NodaTime;
using System;

namespace StorageBackend
{
	[Obsolete]
	public class PageModel
	{
		/// <summary>
		///     The unique id
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// The unique Link.
		/// </summary>
		public string Link { get; set; }

		/// <summary>
		///     The markdown formatted content
		/// </summary>
		[NotNull]
		public string Content { get; set; }

		/// <summary>
		///     Date of Creation
		/// </summary>
		public Instant CreationTime { get; set; }

		/// <summary>
		///     Date of last Change
		/// </summary>
		public Instant ChangeTime { get; set; }

		public bool IsFavorite { get; set; } = false;

		/// <summary>
		///     Indicates whether this page can be edited
		/// </summary>
		public bool IsLocked { get; set; } = false;

		public string ExternalFileToken { get; set; } = null;

		public Instant? ExternalFileImportDate { get; set; }

		public PageModel([NotNull] Guid id, [NotNull] string link, [NotNull] string content, [NotNull] IClock clock)
		{
			Id = id;
			Link = link;
			Content = content;
			var now = clock.GetCurrentInstant();
			CreationTime = now;
			ChangeTime = now;
		}

		public PageModel([NotNull] Guid id, [NotNull] string link, [NotNull] string content, Instant creationTime, Instant changeTime)
		{
			Id = id;
			Link = link;
			Content = content;
			CreationTime = creationTime;
			ChangeTime = changeTime;
		}

		public PageModel([NotNull] Guid id, [NotNull] string link, [NotNull] string content, [NotNull] IClock clock,
			[NotNull] string externalFileToken, Instant externalFileImportDate)
		{
			Id = id;
			Link = link;
			Content = content;
			var now = clock.GetCurrentInstant();
			CreationTime = now;
			ChangeTime = now;
			ExternalFileToken = externalFileToken;
			ExternalFileImportDate = ExternalFileImportDate;
		}
	}
}
