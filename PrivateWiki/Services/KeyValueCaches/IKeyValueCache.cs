using System.Threading.Tasks;
using FluentResults;
using PrivateWiki.Services.SerializationService;

namespace PrivateWiki.Services.KeyValueCaches
{
	public interface IKeyValueCache
	{
		public Task<Result> InsertAsync(string key, int value);

		public Task<Result<int>> GetIntAsync(string key);

		public Task<Result> InsertAsync(string key, string value);

		public Task<Result<string>> GetStringAsync(string key);

		public Task<Result> InsertAsync(string key, bool value);

		public Task<Result<bool>> GetBooleanAsync(string key);

		public Task<Result> InsertAsync<T>(string key, T value);

		public Task<Result<T>> GetObjectAsync<T>(string key);

		public Task<Result> InsertAsync<T>(string key, T value, IJsonSerializationService<T> serializer);

		public Task<Result<T>> GetObjectAsync<T>(string key, IJsonSerializationService<T> deserializer);

		public Task<Result> RemoveAsync(string key);

		public Task<Result> RemoveAllAsync();

		public Task DeleteCache();
	}

	public interface IInMemoryKeyValueCache : IKeyValueCache
	{
	}

	public interface IPersistentKeyValueCache : IKeyValueCache
	{
	}
}