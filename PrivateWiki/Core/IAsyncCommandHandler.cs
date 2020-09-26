using System.Threading.Tasks;

namespace PrivateWiki.Core
{
	public interface IAsyncCommandHandler<in T>
	{
		public Task Execute(T command);
	}
}