using PrivateWiki.DAL;
using SimpleInjector;

namespace PrivateWiki.UWP.CompositionRoot
{
	public static class CompositionRoot
	{
		public static void Bootstrap(Container container)
		{
			DALCompositionRoot.Bootstrap(container);
			PrivateWiki.CompositionRoot.Bootstrap(container);
			Dependencies.CompositionRoot.Bootstrap(container);
		}
	}
}