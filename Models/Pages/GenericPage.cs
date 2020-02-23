using System;
using System.Collections.Generic;
using System.Text;
using NodaTime;

namespace Models.Pages
{
	public class GenericPage : Page
	{
		public new string ContentType;

		public override string GetContentType() => "unkwon";
	}
}
