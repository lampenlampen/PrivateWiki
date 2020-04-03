using System;
using System.Runtime.Serialization;
using PrivateWiki.Data;

namespace PrivateWiki
{
	public static class AppConfig
	{
		public static string[] SupportedContentTypes = Enum.GetNames(typeof(ContentType));
	}
}