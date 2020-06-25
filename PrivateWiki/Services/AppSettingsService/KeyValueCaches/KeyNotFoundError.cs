using FluentResults;

namespace PrivateWiki.Services.AppSettingsService.KeyValueCaches
{
	public class KeyNotFoundError : Error
	{
		public KeyNotFoundError(string key) : base($"Key not found: {key}")
		{
		}
	}
}