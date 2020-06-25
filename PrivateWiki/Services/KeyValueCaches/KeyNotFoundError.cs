using FluentResults;

namespace PrivateWiki.Services.KeyValueCaches
{
	public class KeyNotFoundError : Error
	{
		public KeyNotFoundError(string key) : base($"Key not found: {key}")
		{
		}
	}
}