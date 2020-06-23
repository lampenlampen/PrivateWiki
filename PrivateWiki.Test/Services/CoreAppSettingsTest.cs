using System;
using System.Linq;
using System.Reactive.Linq;
using PrivateWiki.Services.AppSettingsService;
using PrivateWiki.Services.AppSettingsService.CoreAppSettings;
using Xunit;
using Xunit.Abstractions;
using Xunit.Repeat;

namespace PrivateWiki.Test.Services
{
	public class CoreAppSettingsTest
	{
		private readonly ITestOutputHelper output;

		public CoreAppSettingsTest(ITestOutputHelper output)
		{
			this.output = output;
		}

		[Theory]
		[Repeat(100)]
		public void InsertGetStringsTest(int iteration)
		{
			var key = RandomString(iteration);
			var value = RandomString(iteration);
			
			var coreAppSettings = new CoreAppSettings();
			coreAppSettings.Insert(key, value);
			var actual = coreAppSettings.GetString(key);
			
			Assert.True(actual.IsSuccess);

			Assert.Equal(value, actual.Value);
		}
		
		private static Random random = new Random();
		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}

		[Theory]
		[Repeat(100)]
		public void InsertGetIntsTest(int iteration)
		{
			var key = RandomString(iteration);
			var value = new Random().Next();
			
			var coreAppSettings = new CoreAppSettings();
			coreAppSettings.Insert(key, value);

			var actual = coreAppSettings.GetInt(key);
			
			Assert.True(actual.IsSuccess);
			Assert.Equal(value, actual.Value);
		}

		[Theory]
		[Repeat(100)]
		public void InsertGetBytesTest(int iteration)
		{
			var key = RandomString(iteration);

			var length = new Random().Next(iteration);
			var value = new byte[iteration];
			new Random().NextBytes(value);
			
			var coreAppSettings = new CoreAppSettings();
			coreAppSettings.InsertBytes(key, value);

			var actual = coreAppSettings.GetBytes(key);

			Assert.Equal(value, actual.Value);

		}
		
		[Fact]
		public void InsertGetObjectsTest()
		{
			var key = RandomString(10);
			
			var value = new TestObject();
			
			var coreAppSettings = new CoreAppSettings();
			coreAppSettings.Insert(key, value);

			var result = coreAppSettings.GetObject<TestObject>(key);
			
			Assert.True(result.IsSuccess);

			var actual = result.Value;
			
			Assert.Equal(value.test1, actual.test1);
			Assert.Equal(value.test2, actual.test2);
			Assert.Equal(value.test3, actual.test3);
		}

		public class TestObject
		{
			public string test1 = "test1String";
			public int test2 = 5;
			public bool test3 = true;
		}
	}
}