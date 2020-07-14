using System.Threading.Tasks;

namespace PrivateWiki.Services.LFSBackupService
{
	public interface ILFSBackupService
	{
		public Task Sync(LFSBackupServiceOptions options);
	}
}