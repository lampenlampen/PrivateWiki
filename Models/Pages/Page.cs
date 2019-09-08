using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Models.Pages
{
	public abstract class Page
	{
		protected Page(WikiLink link, Guid id)
		{
			Id = id;
			Link = link;
		}

		public WikiLink Link { get; }
		
		public Guid Id { get; }
		
		public string Content { get; }
		
		public DateTimeOffset Created { get; }
		
		public DateTimeOffset LastChanged { get; }
		
		public bool IsLocked { get; }

		public CultureInfo locale { get; }
	}
}