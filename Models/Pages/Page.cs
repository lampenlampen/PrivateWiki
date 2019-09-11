using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NodaTime;

namespace Models.Pages
{
	public abstract class Page
	{
		protected Page(string link, Guid id, string content, Instant created, Instant lastChanged, bool isLocked)
		{
			Link = link;
			Id = id;
			Content = content;
			Created = created;
			LastChanged = lastChanged;
			IsLocked = isLocked;
		}

		public string Link { get; }
		
		public Guid Id { get; }
		
		public string Content { get; }
		
		public Instant Created { get; }
		
		public Instant LastChanged { get; }
		
		public bool IsLocked { get; }
	}
}