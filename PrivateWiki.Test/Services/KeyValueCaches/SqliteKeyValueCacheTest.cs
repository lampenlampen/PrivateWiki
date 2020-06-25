using PrivateWiki.Services.KeyValueCaches;
using PrivateWiki.Services.SqliteStorage;
using PrivateWiki.Test.Utilities;
using Xunit;
using Xunit.Repeat;

namespace PrivateWiki.Test.Services.KeyValueCaches
{
	public class SqliteKeyValueCacheTest
	{
		[Theory]
		[Repeat(1)]
		public async void InsertAndGetStringTest(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			var coreAppSettings = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions() {Path = "C:\\Users\\felix"}));
			await coreAppSettings.InsertAsync(key, value);
			var actual = await coreAppSettings.GetStringAsync(key);

			Assert.True(actual.IsSuccess);

			Assert.Equal(value, actual.Value);
		}

		[Theory]
		[Repeat(1)]
		public async void GetKeyNotInDb(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			var coreAppSettings = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions() {Path = "C:\\Users\\felix"}));
			await coreAppSettings.InsertAsync(key, value);
			var actual = await coreAppSettings.GetStringAsync(key + "d");

			Assert.True(actual.IsFailed);
			Assert.True(actual.HasError<KeyNotFoundError>());
		}

		[Theory]
		[Repeat(1)]
		public async void InsertDuplicateKeyValuePairs(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			var coreAppSettings = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions() {Path = "C:\\Users\\felix"}));
			await coreAppSettings.InsertAsync(key, value);
			await coreAppSettings.InsertAsync(key, value);

			var actual = await coreAppSettings.GetStringAsync(key);

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

			var coreAppSettings = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions() {Path = "C:\\Users\\felix"}));
			await coreAppSettings.InsertAsync(key, value);
			await coreAppSettings.InsertAsync(key, value2);

			var actual = await coreAppSettings.GetStringAsync(key);

			Assert.True(actual.IsSuccess);

			Assert.Equal(value2, actual.Value);
		}

		[Theory]
		[Repeat(1)]
		public async void UseIndexerToInsertAndGet(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			var coreAppSettings = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions() {Path = "C:\\Users\\felix"}));
			coreAppSettings[key] = value;

			var actual = coreAppSettings[key];


			Assert.Equal(value, actual);
		}

		[Fact]
		public async void InsertGetObjectsTest()
		{
			var key = RandomStringGenerator.RandomString(10);

			var value = new TestObject();

			var coreAppSettings = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions() {Path = "C:\\Users\\felix"}));
			await coreAppSettings.InsertAsync(key, value);

			var result = await coreAppSettings.GetObjectAsync<TestObject>(key);

			Assert.True(result.IsSuccess);

			var actual = result.Value;

			Assert.Equal(value.test1, actual.test1);
			Assert.Equal(value.test2, actual.test2);
			Assert.Equal(value.test3, actual.test3);
		}
	}
}