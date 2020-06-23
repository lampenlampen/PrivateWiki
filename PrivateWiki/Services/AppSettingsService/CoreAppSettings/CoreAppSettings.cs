using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using FluentResults;
using NLog;

namespace PrivateWiki.Services.AppSettingsService.CoreAppSettings
{
	public class CoreAppSettings : ICoreAppSettings
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		private Dictionary<string, byte[]> _dict;

		public CoreAppSettings()
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

		private byte[]? Get(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					// TODO Key not found
				}	
			}

			return value;
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

		public bool Remove(string key)
		{
			lock (_dict)
			{
				_dict.Remove(key);
			}

			return true;
		}

		public bool RemoveAll()
		{
			lock (_dict)
			{
				_dict.Clear();
			}

			return true;
		}
		
		public Result Insert(string key, string value)
		{
			Insert(key, Encoding.UTF8.GetBytes(value));
			
			return Result.Ok();
		}

		public Result<string> GetString(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					return Result.Fail(new KeyNotFoundError(key));
				}	
			}
			
			return Result.Ok(Encoding.UTF8.GetString(value));
		}

		public Result Insert(string key, int value)
		{
			byte[] intBytes = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
				Array.Reverse(intBytes);

			Insert(key, intBytes);
			
			return Result.Ok();
		}

		public Result<int> GetInt(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					return Result.Fail(new KeyNotFoundError(key));
				}	
			}
			
			if (BitConverter.IsLittleEndian)
				Array.Reverse(value);

			return Result.Ok(BitConverter.ToInt32(value, 0));
		}

		public Result Insert(string key, bool value)
		{
			Insert(key, BitConverter.GetBytes(value));
			
			return Result.Ok();
		}

		public Result<bool> GetBoolean(string key)
		{
			byte[] value;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out value))
				{
					return Result.Fail(new KeyNotFoundError(key));
				}	
			}

			return Result.Ok(BitConverter.ToBoolean(value, 0));
		}

		public Result Insert<T>(string key, T value)
		{
			var data = Serialize(value);

			lock (_dict)
			{
				_dict[key] = data;
			}

			return Result.Ok();
		}

		public Result<T> GetObject<T>(string key)
		{
			byte[] data;

			lock (_dict)
			{
				if (!_dict.TryGetValue(key, out data))
				{
					return Result.Fail(new KeyNotFoundError(key));
				}	
			}

			var value = Deserialize<T>(data);

			return Result.Ok(value);
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
	
	public class KeyNotFoundError : Error
	{
		public KeyNotFoundError(string key) : base($"Key not found: {key}")
		{
		}
	}
}