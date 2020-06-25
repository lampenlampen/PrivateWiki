using System;
using PrivateWiki.Test.Utilities;
using Xunit;
using Xunit.Abstractions;
using Xunit.Repeat;

namespace PrivateWiki.Test.Services.KeyValueCaches
{
	public class InMemoryCache
	{
		private readonly ITestOutputHelper output;

		public InMemoryCache(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Theory]
		[Repeat(100)]
		public async void InsertGetStringsTest(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = RandomStringGenerator.RandomString(iteration);

			var coreAppSettings = new PrivateWiki.Services.KeyValueCaches.InMemoryCache();
			await coreAppSettings.InsertAsync(key, value);
			var actual = await coreAppSettings.GetStringAsync(key);

			Assert.True(actual.IsSuccess);

			Assert.Equal(value, actual.Value);
		}

		[Theory]
		[Repeat(100)]
		public async void InsertGetIntsTest(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);
			var value = new Random().Next();

			var coreAppSettings = new PrivateWiki.Services.KeyValueCaches.InMemoryCache();
			await coreAppSettings.InsertAsync(key, value);

			var actual = await coreAppSettings.GetIntAsync(key);

			Assert.True(actual.IsSuccess);
			Assert.Equal(value, actual.Value);
		}

		[Theory]
		[Repeat(100)]
		public void InsertGetBytesTest(int iteration)
		{
			var key = RandomStringGenerator.RandomString(iteration);

			var length = new Random().Next(iteration);
			var value = new byte[iteration];
			new Random().NextBytes(value);

			var coreAppSettings = new PrivateWiki.Services.KeyValueCaches.InMemoryCache();
			coreAppSettings.InsertBytes(key, value);

			var actual = coreAppSettings.GetBytes(key);

			Assert.Equal(value, actual.Value);
		}

		[Fact]
		public async void InsertGetObjectsTest()
		{
			var key = RandomStringGenerator.RandomString(10);

			var value = new TestObject();

			var coreAppSettings = new PrivateWiki.Services.KeyValueCaches.InMemoryCache();
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