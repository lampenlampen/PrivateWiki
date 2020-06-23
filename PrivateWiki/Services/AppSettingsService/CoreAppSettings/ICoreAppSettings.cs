using FluentResults;

namespace PrivateWiki.Services.AppSettingsService.CoreAppSettings
{
	public interface ICoreAppSettings
	{
		public Result InsertBytes(string key, byte[] value);
		
		public Result<byte[]> GetBytes(string key);

		public Result Insert(string key, int value);
		
		public Result<int> GetInt(string key);

		public Result Insert(string key, string value);
		
		public Result<string> GetString(string key);
		
		public Result Insert(string key, bool value);

		public Result<bool> GetBoolean(string key);

		public Result Insert<T>(string key, T value);

		public Result<T> GetObject<T>(string key);

		public bool Remove(string key);

		public bool RemoveAll();
	}
}