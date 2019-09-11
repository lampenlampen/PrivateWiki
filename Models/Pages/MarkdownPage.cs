﻿using System;
using System.Collections.Generic;
using System.Text;
using NodaTime;

namespace Models.Pages
{
	public class MarkdownPage : Page
	{
		public MarkdownPage(string link, Guid id, string content, Instant created, Instant lastChanged, bool isLocked) : base(link, id, content, created, lastChanged, isLocked)
		{
		}
	}
}
