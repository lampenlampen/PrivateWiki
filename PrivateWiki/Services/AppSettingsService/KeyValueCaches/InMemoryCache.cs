using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentResults;
using NLog;

namespace PrivateWiki.Services.AppSettingsService.KeyValueCaches
{
	public class InMemoryCache : IInMemoryKeyValueCache
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		private Dictionary<string, byte[]> _dict;

		public InMemoryCache()
		{
			_dict = new Dictionary<string, byte[]>();
		}
		
		private bool Insert(string key, byte[] value)
		{
			lock (_dict)
			{
				_dict[key] = value;
			}

			return true;
		}

		public Result InsertBytes(string key, byte[] value)
		{
			Insert(key, value);

			return Result.Ok();
		}

		public Result<byte[]> GetBytes(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					Result.Fail(new KeyNotFoundError(key));
				}	
			}

			return Result.Ok(value);
		}

		public Task<Result> RemoveAsync(string key)
		{
			lock (_dict)
			{
				_dict.Remove(key);
			}

			return Task.FromResult(Result.Ok());
		}

		public Task<Result> RemoveAllAsync()
		{
			lock (_dict)
			{
				_dict.Clear();
			}

			return Task.FromResult(Result.Ok());
		}
		
		public Task<Result> InsertAsync(string key, string value)
		{
			Insert(key, Encoding.UTF8.GetBytes(value));
			
			return Task.FromResult(Result.Ok());
		}

		public Task<Result<string>> GetStringAsync(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					return Task.FromResult(Result.Fail<string>(new KeyNotFoundError(key)));
				}	
			}
			
			return Task.FromResult(Result.Ok(Encoding.UTF8.GetString(value)));
		}

		public Task<Result> InsertAsync(string key, int value)
		{
			byte[] intBytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
				Array.Reverse(intBytes);

			Insert(key, intBytes);
			
			return Task.FromResult(Result.Ok());
		}

		public Task<Result<int>> GetIntAsync(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					return Task.FromResult(Result.Fail<int>(new KeyNotFoundError(key)));
				}	
			}
			
			if (BitConverter.IsLittleEndian)
				Array.Reverse(value);

			return Task.FromResult(Result.Ok(BitConverter.ToInt32(value, 0)));
		}

		public Task<Result> InsertAsync(string key, bool value)
		{
			Insert(key, BitConverter.GetBytes(value));
			
			return Task.FromResult(Result.Ok());
		}

		public Task<Result<bool>> GetBooleanAsync(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					return Task.FromResult(Result.Fail<bool>(new KeyNotFoundError(key)));
				}	
			}

			return Task.FromResult(Result.Ok(BitConverter.ToBoolean(value, 0)));
		}

		public Task<Result> InsertAsync<T>(string key, T value)
		{
			var data = Serialize(value);

			lock (_dict)
			{
				_dict[key] = data;
			}

			return Task.FromResult(Result.Ok());
		}

		public Task<Result<T>> GetObjectAsync<T>(string key)
		{
			byte[] data;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out data))
				{
					return Task.FromResult(Result.Fail<T>(new KeyNotFoundError(key)));
				}	
			}

			var value = Deserialize<T>(data);

			return Task.FromResult(Result.Ok(value));
		}

		private byte[] Serialize<T>(T value)
		{
			// TODO Serialize object to byte[]

			var json = JsonSerializer.SerializeToUtf8Bytes(value);

			return json;
		}

		private T Deserialize<T>(byte[] data)
		{
			// TODO Deserialize byte[]-data to object

			var value = JsonSerializer.Deserialize<T>(data);

			return value;
		}
	}
}