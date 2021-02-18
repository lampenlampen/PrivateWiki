using System.Globalization;

namespace PrivateWiki.Core.Events
{
	public class CultureChangedEvent : Event<CultureChangedEventArgs> { }

	public class CultureChangedEventArgs
	{
		public CultureChangedEventArgs(CultureInfo newCulture)
		{
			NewCulture = newCulture;
		}

		public CultureInfo NewCulture { get; }
	}
}