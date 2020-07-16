using System.Threading.Tasks;

namespace PrivateWiki.Services.LFSBackupService
{
	public interface ILFSBackupService
	{
		public Task ExportAsync(LFSBackupServiceOptions options);

		public Task CreateBackupAsync(LFSBackupServiceOptions options);
	}
}