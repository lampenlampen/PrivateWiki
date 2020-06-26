using System;
using PrivateWiki.Services.KeyValueCaches;
using PrivateWiki.Services.SqliteStorage;
using PrivateWiki.Test.Utilities;
using Xunit;
using Xunit.Repeat;

namespace PrivateWiki.Test.Services.KeyValueCaches
{
	public class SqliteKeyValueCacheTest : IDisposable
	{
		private readonly string _dbPath;
		private readonly SqliteKeyValueCache _cache;

		public SqliteKeyValueCacheTest()
		{
			_dbPath = Guid.NewGuid().ToString();
			_cache = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions {Path = $"{_dbPath}.db"}));
		}

		[Theory]
		[Repeat(1)]
		public async void InsertAndGetStringTest(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			await _cache.InsertAsync(key, value);
			var actual = await _cache.GetStringAsync(key);

			Assert.True(actual.IsSuccess);

			Assert.Equal(value, actual.Value);
		}

		[Theory]
		[Repeat(1)]
		public async void GetKeyNotInDb(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			await _cache.InsertAsync(key, value);
			var actual = await _cache.GetStringAsync(key + "d");

			Assert.True(actual.IsFailed);
			Assert.True(actual.HasError<KeyNotFoundError>());
		}

		[Theory]
		[Repeat(1)]
		public async void InsertDuplicateKeyValuePairs(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			await _cache.InsertAsync(key, value);
			await _cache.InsertAsync(key, value);

			var actual = await _cache.GetStringAsync(key);

			Assert.True(actual.IsSuccess);

			Assert.Equal(value, actual.Value);
		}

		[Theory]
		[Repeat(1)]
		public async void InsertDuplicateKeysWithDifferentValues(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);
			var value2 = RandomStringGenerator.RandomString(iteration);

			await _cache.InsertAsync(key, value);
			await _cache.InsertAsync(key, value2);

			var actual = await _cache.GetStringAsync(key);

			Assert.True(actual.IsSuccess);

			Assert.Equal(value2, actual.Value);
		}

		[Theory]
		[Repeat(1)]
		public async void UseIndexerToInsertAndGet(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			_cache[key] = value;

			var actual = _cache[key];


			Assert.Equal(value, actual);
		}

		[Fact]
		public async void InsertGetObjectsTest()
		{
			var key = RandomStringGenerator.RandomString(10);

			var value = new TestObject();

			await _cache.InsertAsync(key, value);

			var result = await _cache.GetObjectAsync<TestObject>(key);

			Assert.True(result.IsSuccess);

			var actual = result.Value;

			Assert.Equal(value.test1, actual.test1);
			Assert.Equal(value.test2, actual.test2);
			Assert.Equal(value.test3, actual.test3);
		}

		public void Dispose()
		{
			_cache.DeleteCache();
		}
	}
}