using System.Threading.Tasks;

namespace PrivateWiki.Services.StartupTask
{
	public interface IStartupTask
	{
		public Task<bool> Execute();
	}
}