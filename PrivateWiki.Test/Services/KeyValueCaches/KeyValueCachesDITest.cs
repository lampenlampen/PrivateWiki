using PrivateWiki.Services.KeyValueCaches;
using Xunit;

namespace PrivateWiki.Test.Services.KeyValueCaches
{
	public class KeyValueCachesDITest
	{
		[Fact]
		public void Test1()
		{
			var container = Application.Instance.Container;

			var caches = container.GetAllInstances<IKeyValueCache>();
			var inMemoryCaches = container.GetInstance<IInMemoryKeyValueCache>();
			var persistentCaches = container.GetInstance<IPersistentKeyValueCache>();
		}
	}
}